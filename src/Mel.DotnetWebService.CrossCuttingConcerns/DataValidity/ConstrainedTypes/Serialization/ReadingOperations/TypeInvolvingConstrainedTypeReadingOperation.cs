using System.Reflection;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

abstract class TypeInvolvingConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static TypeInvolvingConstrainedTypeReadingOperation InstanceSuitedFor(Type type)
	=> type switch
	{
		// var t when t.IsAFirstClassCollection(out var constructorParameterType, out var instanciationOperationParameterTypeCandidates) => FirstClassCollectionReadingOperation.Instance(type, constructorParameterType, instanciationOperationParameterTypeCandidates),
		// var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor) => PositionalRecordReadingOperation.Instance(type, propertyBasedConstructor!),
		// var t when t.IsDeclaredAsAGetSetStyleClass() => GetSetStyleClassReadingOperation.Instance,
		// _ => ClassWithConstructorAndSettersReadingOperation.Instance
		var t when t.IsAFirstClassCollectionWithIConstrainedCollection(out var instanciationOperationParameterTypeCandidates) => FirstClassCollectionReadingOperation.Instance(type, instanciationOperationParameterTypeCandidates),
		var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor)                                          => PositionalRecordReadingOperation.Instance(type, propertyBasedConstructor!),
		var t when t.IsClassWithPublicConstructorHavingParameters(out var constructorsWithParameters)                         => ClassWithPublicConstructorHavingParametersReadingOperation.Instance(type, constructorsWithParameters),
		var t when t.IsClassWithPublicParameterlessConstructorAndSetters(out var settableProperties)                          => ClassWithPublicParameterlessConstructorAndSettersReadingOperation.Instance(type, settableProperties),
		var t when t.IsClassWithPublicStaticFactoryMethod(out var staticFactoryMethods)                                       => ClassWithPublicStaticFactoryMethodReadingOperation.Instance(type, staticFactoryMethods),
		// var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(out var singletonAccessProperty)                 => ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyReadingOperation.Instance(type, singletonAccessProperty!),
		// var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughField(out var singletonAccessProperty)                    => ClassWithSinglePublicAccessToSingletonInstanceThroughFieldReadingOperation.Instance(type, singletonAccessProperty!),
		// var t when t.IsClassWithPublicInstancesExposedAsStaticReadonlyFields(out var staticReadonlyInstanceAccessFields)              => ClassWithPublicInstancesExposedAsFieldsReadingOperation.Instance(type, staticReadonlyInstanceAccessFields),
		// var t when t.IsClassWithPublicInstancesExposedAsProperties(out var staticInstanceAccessProperties)                            => ClassWithPublicInstancesExposedAsPropertiesReadingOperation.Instance(type, staticInstanceAccessProperties),
		// var t when t.IsClassWithPublicInstancesExposedAsStaticFields(out var staticInstanceAccessFields)                              => ClassWithPublicInstancesExposedAsFieldsReadingOperation.Instance(type, staticInstanceAccessFields),
		_ => throw new InvalidOperationException($"{nameof(TypeInvolvingConstrainedTypeReadingOperation)}: Cannot resolve an {nameof(TypeInvolvingConstrainedTypeReadingOperation)} for type {type.GetName()}.")
	};

	protected object?[] ReadParameterValuesFor(MethodBase constructorOrMethod, ref Utf8JsonReader reader, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var constructorParameterInfos = constructorOrMethod.GetParameters().ToList();

		var constructorParameterValues = new object?[constructorParameterInfos.Count];
		var missingDeserializedParameterNames = constructorParameterInfos.Select(p => p.Name).ToList();
		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				break;
			}

			var propertyName = reader.GetString();
			var matchingParameterIndex = -1;
			matchingParameterIndex = constructorParameterInfos.FindIndex(pn => pn.Name.Matches(propertyName));

			reader.Read();
			var matchingParameterType = constructorParameterInfos[matchingParameterIndex].ParameterType;
			var deserializedParameter =
				For(matchingParameterType)
					.Execute(ref reader, matchingParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			constructorParameterValues[matchingParameterIndex] = deserializedParameter;
			missingDeserializedParameterNames.RemoveAll(parameterName => parameterName.Matches(propertyName));
		}

		return constructorParameterValues;
	}
	//=> constructorOrMethod.GetParameters() switch
	//{
	//	[] => [],
	//	[var singleParameter] => [ExampleValueGenerator.GenerateExampleOf(singleParameter.ParameterType, reader)],
	//	var multipleParameters => GenerateMultipleParameters(ArrayWithAtLeast2Items.ApplyConstraintsTo(multipleParameters), reader)
	//};

}

class ClassWithPublicStaticFactoryMethodReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly ClassWithPublicStaticFactoryMethodReadingOperation _instance = new();
	static readonly Dictionary<Type, MethodInfo> StaticFactoryMethodWithMostParametersByType = [];
	public static ClassWithPublicStaticFactoryMethodReadingOperation Instance(Type type, MethodInfo[] staticFactoryMethods)
	{
		if (!StaticFactoryMethodWithMostParametersByType.ContainsKey(type))
		{
			var staticFactoryMethodWithMostParameters = staticFactoryMethods
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First();
			StaticFactoryMethodWithMostParametersByType.Add(type, staticFactoryMethodWithMostParameters);
		}
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var staticFactoryMethod = StaticFactoryMethodWithMostParametersByType[targetType];
		var parameters = ReadParameterValuesFor(staticFactoryMethod, ref reader, options, preComputedOptionsWithoutConstrainedTypeConverter);

		var instance = staticFactoryMethod.Invoke(null, parameters);
		return instance;
	}
}

class ClassWithPublicParameterlessConstructorAndSettersReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly ClassWithPublicParameterlessConstructorAndSettersReadingOperation _instance = new();
	static readonly Dictionary<Type, List<PropertyInfo>> SettablePropertiesByType = new();
	public static ClassWithPublicParameterlessConstructorAndSettersReadingOperation Instance(Type type, IEnumerable<PropertyInfo> settableProperties)
	{
		SettablePropertiesByType.TryAdd(type, settableProperties.ToList());
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instanceUnderConstruction = Activator.CreateInstance(targetType)!;
		var allTheSetProperties = SettablePropertiesByType[targetType];

		var setPropertiesValues = new object?[allTheSetProperties.Count];
		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				break;
			}

			var propertyName = reader.GetString();
			var matchingProperty = allTheSetProperties.FirstOrDefault(p => p.Name.Matches(propertyName));

			reader.Read();
			if (matchingProperty is null)
			{
				continue;
			}

			var matchingPropertyType = matchingProperty.PropertyType;
			var deserializedParameter =
				For(matchingPropertyType)
					.Execute(ref reader, matchingPropertyType, options, preComputedOptionsWithoutConstrainedTypeConverter)!;

			SetPropertyValue(matchingProperty, deserializedParameter, instanceUnderConstruction);
		}

		return instanceUnderConstruction;
	}

	void SetPropertyValue(PropertyInfo settableProperty, object value, object instance)
	{
		try
		{
			settableProperty.SetValue(instance, value, null);
		}
		catch
		{
		}
	}
}

class ClassWithPublicConstructorHavingParametersReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly ClassWithPublicConstructorHavingParametersReadingOperation _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> ConstructorWithMostParametersByType = [];
	public static ClassWithPublicConstructorHavingParametersReadingOperation Instance(Type type, ConstructorInfo[] constructorsWithParameters)
	{
		if (!ConstructorWithMostParametersByType.ContainsKey(type))
		{
			var constructorWithMostParameters = constructorsWithParameters
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First();
			ConstructorWithMostParametersByType.Add(type, constructorWithMostParameters);
		}
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var constructor = ConstructorWithMostParametersByType[targetType];
		var parameters = ReadParameterValuesFor(constructor, ref reader, options, preComputedOptionsWithoutConstrainedTypeConverter);

		var instance = constructor.Invoke(parameters);
		return instance;
	}
}
