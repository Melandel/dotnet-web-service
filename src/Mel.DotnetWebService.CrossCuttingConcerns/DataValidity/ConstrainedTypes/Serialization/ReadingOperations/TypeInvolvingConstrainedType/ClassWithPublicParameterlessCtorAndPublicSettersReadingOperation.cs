using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

class ClassWithPublicParameterlessConstructorAndPublicSettersReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly ClassWithPublicParameterlessConstructorAndPublicSettersReadingOperation _instance = new();
	static readonly ConcurrentDictionary<Type, CompiledInvokable> InstantiatorsByType = [];
	static readonly ConcurrentDictionary<Type, List<PropertyInfo>> PublicSettablePropertiesByType = new();
	public static ClassWithPublicParameterlessConstructorAndPublicSettersReadingOperation Instance(Type type, IEnumerable<PropertyInfo> publicSettableProperties)
	{
		InstantiatorsByType.GetOrAdd(type, _ => BuildInstantiatorUsingParameterlessConstructor(type));
		PublicSettablePropertiesByType.GetOrAdd(type, _ => publicSettableProperties.ToList());
		return _instance;
	}

	static CompiledInvokable BuildInstantiatorUsingParameterlessConstructor(Type type)
	=> CompiledInvokable.FromExpression(Expression.New(type.GetConstructors().First()));

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instanceUnderConstruction = InstantiatorsByType[targetType].Invoke();
		var allTheSetProperties = PublicSettablePropertiesByType[targetType];

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
