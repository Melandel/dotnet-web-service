using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

abstract class ParentObjectExampleInstantiationBuilder : ExampleRegenerationBuilder
{
	static readonly ConcurrentDictionary<Type, ParentObjectExampleInstantiationBuilder> ParentObjectGeneratorsByType = [];

	public static ParentObjectExampleInstantiationBuilder InstanceSuitedFor(Type type)
	=> ParentObjectGeneratorsByType.GetOrAdd(type, _ => type switch
	{
		var t when ConstrainedTypeInfos.Include(t) => ConstrainedTypeExampleInstantiationBuilder.Instance,
		var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor) => PositionalRecordParentObjectExampleInstantiationBuilder.Instance(type, propertyBasedConstructor),
		var t when t.IsClassWithPublicConstructorHavingParameters(out var constructorsWithParameters) => ClassWithPublicConstructorHavingParametersExampleInstantiationBuilder.Instance(type, constructorsWithParameters),
		var t when t.IsClassWithPublicParameterlessConstructorAndPublicSetters(out var settableProperties) => ClassWithPublicParameterlessConstructorAndSettersExampleInstantiationBuilder.Instance(type, settableProperties),
		var t when t.IsClassWithPublicStaticFactoryMethod(out var staticFactoryMethods) => ClassWithPublicStaticFactoryMethodExampleInstantiationBuilder.Instance(type, staticFactoryMethods),
		var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(out var singletonAccessProperty) => ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstantiationBuilder.Instance(type, singletonAccessProperty),
		var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughField(out var singletonAccessProperty) => ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstantiationBuilder.Instance(type, singletonAccessProperty),
		var t when t.IsClassWithPublicInstancesExposedAsStaticReadonlyFields(out var staticReadonlyInstanceAccessFields) => ClassWithPublicInstancesExposedAsFieldsExampleInstantiationBuilder.Instance(type, staticReadonlyInstanceAccessFields),
		var t when t.IsClassWithPublicInstancesExposedAsProperties(out var staticInstanceAccessProperties) => ClassWithPublicInstancesExposedAsPropertiesExampleInstantiationBuilder.Instance(type, staticInstanceAccessProperties),
		var t when t.IsClassWithPublicInstancesExposedAsStaticFields(out var staticInstanceAccessFields) => ClassWithPublicInstancesExposedAsFieldsExampleInstantiationBuilder.Instance(type, staticInstanceAccessFields),
		_ => throw new InvalidOperationException($"{nameof(ParentObjectExampleInstantiationBuilder)}: Cannot resolve an {nameof(RandomizedExampleRegeneratorUsingExpressionTrees)} for type {type.GetName()}.")
	});

	protected Expression[] GenerateParameterExamplesFor(MethodBase constructorOrMethod, ParameterExpression generativeSalts)
	=> constructorOrMethod.GetParameters() switch
	{
		[] => [],
		[var singleParameter] => GenerateSingleParameter(singleParameter, generativeSalts),
		var multipleParameters => GenerateMultipleParameters(ArrayWithAtLeast2Items.ApplyConstraintsTo(multipleParameters), generativeSalts)
	};

	Expression[] GenerateSingleParameter(ParameterInfo singleConstructionParameter, ParameterExpression generativeSalts)
	{
		var parameter = ExampleRegenerationBuilder.BuildFor(
			singleConstructionParameter.ParameterType,
			generativeSalts);
		return [parameter];
	}
	Expression[] GenerateMultipleParameters(ArrayWithAtLeast2Items<ParameterInfo> constructionParameters, ParameterExpression generativeSalts)
	{
		var parameters = new List<Expression>(constructionParameters.Length);
		var saltOffsetByType = new Dictionary<Type, int>();
		foreach (var p in constructionParameters)
		{
			var parameterExampleValue = ExampleRegenerationBuilder.BuildFor(p.ParameterType, generativeSalts);
			parameters.Add(parameterExampleValue);
		}

		return parameters.ToArray();
	}
}
