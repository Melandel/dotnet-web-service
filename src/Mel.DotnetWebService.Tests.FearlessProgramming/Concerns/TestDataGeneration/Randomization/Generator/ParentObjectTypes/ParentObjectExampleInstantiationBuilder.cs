using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.NullabilityInspection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization.Generator;

abstract class ParentObjectExampleInstantiationBuilder : RandomizedExampleInstantiationBuilder
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
		_ => throw new InvalidOperationException($"{nameof(ParentObjectExampleInstantiationBuilder)}: Cannot resolve an {nameof(RandomizedExampleInstantiationBuilder)} for type {type.GetName()}.")
	});

	protected Expression[] GenerateParameterExamplesFor(MethodBase constructorOrMethod, Expression generativeSalts)
	=> constructorOrMethod.GetParameters()
		.Select(p => GenerateParameter(p, constructorOrMethod, generativeSalts))
		.ToArray();

	BlockExpression GenerateParameter(ParameterInfo parameter, MethodBase constructorOrMethod, Expression generativeSalts)
	=> NullabilityResolver.Resolve(parameter, constructorOrMethod)
		? BuildNullableInstantiationExpressionFor(parameter.ParameterType, generativeSalts)
		: BuildNonNullInstantiationExpressionFor(parameter.ParameterType, generativeSalts);

	BlockExpression BuildNullableInstantiationExpressionFor(Type type, Expression generativeSalts)
	{
		var random = Expression.Variable(RandomSalt.Type, "rollForNullReference");
		var parameterVar = Expression.Variable(type, "arg");
		var parameter = Expression.Block(
			[random, parameterVar],
			Expression.Assign(random, RandomSalt.SelectBetweenOneAndMeanRecurrenceIntervalForNullGeneration),
			Expression.IfThenElse(
				Expression.Equal(random, RandomSalt.ConstantLowestPossibleValue),
				Expression.Assign(parameterVar, BuildNullExpressionFor(type, generativeSalts)),
				Expression.Assign(parameterVar, BuildNonNullInstantiationExpressionFor(type, generativeSalts))),
			parameterVar);
		return parameter;
	}

	BlockExpression BuildNullExpressionFor(Type type, Expression generativeSalts)
	=> Expression.Block(
		GenerativeSalts.Enqueue(generativeSalts, GenerativeSalts.MarkerForNullReference),
		Expression.Constant(null, type));

	BlockExpression BuildNonNullInstantiationExpressionFor(Type parameterType, Expression generativeSalts)
	{
		var newSalt = Expression.Variable(RandomSalt.Type);
		return Expression.Block(
		[newSalt],
		Expression.Assign(newSalt, RandomSalt.Select),
		RandomizedExampleInstantiationBuilder.BuildFor(
			parameterType,
			newSalt,
			generativeSalts));
	}
}
