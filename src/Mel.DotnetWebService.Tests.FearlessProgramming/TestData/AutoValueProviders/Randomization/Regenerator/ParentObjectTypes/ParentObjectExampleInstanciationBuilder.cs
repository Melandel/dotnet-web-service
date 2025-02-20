using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

abstract class ParentObjectExampleInstanciationBuilder : ExampleRegenerationBuilder
{
	static readonly Dictionary<Type, ParentObjectExampleInstanciationBuilder> ParentObjectGeneratorsByType = [];

	public static ParentObjectExampleInstanciationBuilder InstanceSuitedFor(Type type)
	{
		if (!ParentObjectGeneratorsByType.TryGetValue(type, out var generator))
		{
			generator = type switch
				{
					var t when ConstrainedTypeInfos.Include(t)                                                                       => ConstrainedTypeExampleInstanciationBuilder.Instance,
					var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor)                                     => PositionalRecordParentObjectExampleInstanciationBuilder.Instance(type, propertyBasedConstructor),
					var t when t.IsClassWithPublicConstructorHavingParameters(out var constructorsWithParameters)                    => ClassWithPublicConstructorHavingParametersExampleInstanciationBuilder.Instance(type, constructorsWithParameters),
					var t when t.IsClassWithPublicParameterlessConstructorAndSetters(out var settableProperties)                     => ClassWithPublicParameterlessConstructorAndSettersExampleInstanciationBuilder.Instance(type, settableProperties),
					var t when t.IsClassWithPublicStaticFactoryMethod(out var staticFactoryMethods)                                  => ClassWithPublicStaticFactoryMethodExampleInstanciationBuilder.Instance(type, staticFactoryMethods),
					var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(out var singletonAccessProperty)    => ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstanciationBuilder.Instance(type, singletonAccessProperty),
					var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughField(out var singletonAccessProperty)       => ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstanciationBuilder.Instance(type, singletonAccessProperty),
					var t when t.IsClassWithPublicInstancesExposedAsStaticReadonlyFields(out var staticReadonlyInstanceAccessFields) => ClassWithPublicInstancesExposedAsFieldsExampleInstanciationBuilder.Instance(type, staticReadonlyInstanceAccessFields),
					var t when t.IsClassWithPublicInstancesExposedAsProperties(out var staticInstanceAccessProperties)               => ClassWithPublicInstancesExposedAsPropertiesExampleInstanciationBuilder.Instance(type, staticInstanceAccessProperties),
					var t when t.IsClassWithPublicInstancesExposedAsStaticFields(out var staticInstanceAccessFields)                 => ClassWithPublicInstancesExposedAsFieldsExampleInstanciationBuilder.Instance(type, staticInstanceAccessFields),
					_ => throw new InvalidOperationException($"{nameof(ParentObjectExampleInstanciationBuilder)}: Cannot resolve an {nameof(RandomizedExampleRegeneratorUsingExpressionTrees)} for type {type.GetName()}.")
				};
			ParentObjectGeneratorsByType.Add(type, generator);
		}
		return generator;
	}

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
