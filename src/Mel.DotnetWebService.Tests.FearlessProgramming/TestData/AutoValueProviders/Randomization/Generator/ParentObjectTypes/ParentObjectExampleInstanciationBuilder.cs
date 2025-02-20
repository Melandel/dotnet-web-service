using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

abstract class ParentObjectExampleInstanciationBuilder : RandomizedExampleInstanciationBuilder
{
	static readonly Dictionary<Type, ParentObjectExampleInstanciationBuilder> ParentObjectGeneratorsByType = [];

	public static ParentObjectExampleInstanciationBuilder InstanceSuitedFor(Type type)
	{
		if (!ParentObjectGeneratorsByType.TryGetValue(type, out var generator))
		{
			generator = type switch
				{
					var t when ConstrainedTypeInfos.Include(t)                                                                       => ConstrainedTypeExampleInstanciationBuilder.Instance,
					var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor)                                     => PositionalRecordParentObjectExampleInstanciationBuilder.Instance(type, propertyBasedConstructor!),
					var t when t.IsClassWithPublicConstructorHavingParameters(out var constructorsWithParameters)                    => ClassWithPublicConstructorHavingParametersExampleInstanciationBuilder.Instance(type, constructorsWithParameters),
					var t when t.IsClassWithPublicParameterlessConstructorAndSetters(out var settableProperties)                     => ClassWithPublicParameterlessConstructorAndSettersExampleInstanciationBuilder.Instance(type, settableProperties),
					var t when t.IsClassWithPublicStaticFactoryMethod(out var staticFactoryMethods)                                  => ClassWithPublicStaticFactoryMethodExampleInstanciationBuilder.Instance(type, staticFactoryMethods),
					var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(out var singletonAccessProperty)    => ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstanciationBuilder.Instance(type, singletonAccessProperty!),
					var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughField(out var singletonAccessProperty)       => ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstanciationBuilder.Instance(type, singletonAccessProperty!),
					var t when t.IsClassWithPublicInstancesExposedAsStaticReadonlyFields(out var staticReadonlyInstanceAccessFields) => ClassWithPublicInstancesExposedAsFieldsExampleInstanciationBuilder.Instance(type, staticReadonlyInstanceAccessFields),
					var t when t.IsClassWithPublicInstancesExposedAsProperties(out var staticInstanceAccessProperties)               => ClassWithPublicInstancesExposedAsPropertiesExampleInstanciationBuilder.Instance(type, staticInstanceAccessProperties),
					var t when t.IsClassWithPublicInstancesExposedAsStaticFields(out var staticInstanceAccessFields)                 => ClassWithPublicInstancesExposedAsFieldsExampleInstanciationBuilder.Instance(type, staticInstanceAccessFields),
					_ => throw new InvalidOperationException($"{nameof(ParentObjectExampleInstanciationBuilder)}: Cannot resolve an {nameof(RandomizedExampleInstanciationBuilder)} for type {type.GetName()}.")
				};
			ParentObjectGeneratorsByType.Add(type, generator);
		}
		return generator;
	}

	protected Expression[] GenerateParameterExamplesFor(MethodBase constructorOrMethod, Expression generativeSalts)
	=> constructorOrMethod.GetParameters() switch
	{
		[] => [],
		[var singleParameter] => GenerateSingleParameter(singleParameter, generativeSalts),
		var multipleParameters => GenerateMultipleParameters(ArrayWithAtLeast2Items.ApplyConstraintsTo(multipleParameters), generativeSalts)
	};

	Expression[] GenerateSingleParameter(ParameterInfo singleConstructionParameter, Expression generativeSalts)
	{
		var newSalt = Expression.Variable(RandomSalt.Type);
		var parameter = Expression.Block(
			[newSalt],
			Expression.Assign(newSalt, RandomSalt.Select),
			RandomizedExampleInstanciationBuilder.BuildFor(
				singleConstructionParameter.ParameterType,
				newSalt,
				generativeSalts));

		return [parameter];
	}
	Expression[] GenerateMultipleParameters(ArrayWithAtLeast2Items<ParameterInfo> constructionParameters, Expression generativeSalts)
	{
		var parameters = new List<Expression>(constructionParameters.Length);
		foreach (var p in constructionParameters)
		{
			var newSalt = Expression.Variable(RandomSalt.Type);
			parameters.Add(
				Expression.Block(
					[newSalt],
					Expression.Assign(newSalt, RandomSalt.Select),
					RandomizedExampleInstanciationBuilder.BuildFor(p.ParameterType, newSalt, generativeSalts)));
		}

		return parameters.ToArray();
	}
}
