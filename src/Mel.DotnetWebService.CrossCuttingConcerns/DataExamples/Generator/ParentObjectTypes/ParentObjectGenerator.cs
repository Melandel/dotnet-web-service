using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public abstract class ParentObjectGenerator : ExampleValueGenerator
{
	static readonly Dictionary<Type, ParentObjectGenerator> ParentObjectGeneratorsByType = [];
	public static ParentObjectGenerator InstanceSuitedFor(Type type)
	{
		if (!ParentObjectGeneratorsByType.TryGetValue(type, out var generator))
		{
			generator = type switch
				{
					var t when ConstrainedTypeInfos.Include(t)                                                                                    => ConstrainedTypeGenerator.Instance,
					var t when t.IsGenericType && ConstrainedTypeInfos.Include(t.GetGenericTypeDefinition())                                      => ConstrainedTypeGenerator.Instance,
					var t when t.IsAFirstClassCollection(out var constructorParameterType, out var instanciationOperationParameterTypeCandidates) => FirstClassCollectionParentObjectGenerator.Instance(type, constructorParameterType, instanciationOperationParameterTypeCandidates),
					var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor)                                                  => PositionalRecordParentObjectGenerator.Instance(type, propertyBasedConstructor!),
					var t when t.IsClassWithPublicConstructorHavingParameters(out var constructorsWithParameters)                                 => ClassWithPublicConstructorHavingParametersGenerator.Instance(type, constructorsWithParameters),
					var t when t.IsClassWithPublicParameterlessConstructorAndSetters(out var settableProperties)                                  => ClassWithPublicParameterlessConstructorAndSettersGenerator.Instance(type, settableProperties),
					var t when t.IsClassWithPublicStaticFactoryMethod(out var staticFactoryMethods)                                               => ClassWithPublicStaticFactoryMethodGenerator.Instance(type, staticFactoryMethods),
					var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(out var singletonAccessProperty)                 => ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyGenerator.Instance(type, singletonAccessProperty!),
					var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughField(out var singletonAccessProperty)                    => ClassWithSinglePublicAccessToSingletonInstanceThroughFieldGenerator.Instance(type, singletonAccessProperty!),
					var t when t.IsClassWithPublicInstancesExposedAsStaticReadonlyFields(out var staticReadonlyInstanceAccessFields)              => ClassWithPublicInstancesExposedAsFieldsGenerator.Instance(type, staticReadonlyInstanceAccessFields),
					var t when t.IsClassWithPublicInstancesExposedAsProperties(out var staticInstanceAccessProperties)                            => ClassWithPublicInstancesExposedAsPropertiesGenerator.Instance(type, staticInstanceAccessProperties),
					var t when t.IsClassWithPublicInstancesExposedAsStaticFields(out var staticInstanceAccessFields)                              => ClassWithPublicInstancesExposedAsFieldsGenerator.Instance(type, staticInstanceAccessFields),
					_ => throw new InvalidOperationException($"{nameof(ParentObjectGenerator)}: Cannot resolve an {nameof(ExampleValueGenerator)} for type {type.GetName()}.")
				};
			ParentObjectGeneratorsByType.Add(type, generator);
		}
		return generator;
	}

	protected object[] GenerateParameterExamplesFor(MethodBase constructorOrMethod, int salt)
	=> constructorOrMethod.GetParameters() switch
	{
		[] => [],
		[var singleParameter] => [ExampleValueGenerator.GenerateExampleOf(singleParameter.ParameterType, salt)],
		var multipleParameters => GenerateMultipleParameters(ArrayWithAtLeast2Items.ApplyConstraintsTo(multipleParameters), salt)
	};

	object[] GenerateMultipleParameters(ArrayWithAtLeast2Items<ParameterInfo> constructionParameters, int salt)
	{
		var parameters = new List<object>(constructionParameters.Length);
		var saltOffsetByType = new Dictionary<Type, int>();
		foreach (var p in constructionParameters)
		{
			if (saltOffsetByType.TryGetValue(p.ParameterType, out var currentSaltOffsetForThisType))
			{
				saltOffsetByType[p.ParameterType] = ++currentSaltOffsetForThisType;
			}
			else
			{
				saltOffsetByType.Add(p.ParameterType, 0);
			}
			var saltOffset = saltOffsetByType[p.ParameterType];
			var parameterExampleValue = ExampleValueGenerator.GenerateExampleOf(p.ParameterType, salt + saltOffset);
			parameters.Add(parameterExampleValue);
		}

		return parameters.ToArray();
	}
}
