using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization.Generator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization.Regenerator;

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
	=> constructorOrMethod.GetParameters()
		.Select(p => GenerateParameter(p.ParameterType, generativeSalts))
		.ToArray();

	BlockExpression GenerateParameter(Type type, ParameterExpression generativeSalts)
	{
		var currentSalt = Expression.Variable(RandomSalt.Type, "currentSaltForPossiblyNullReference");
		var parameterVar = Expression.Variable(type, "arg");
		return Expression.Block(
			[currentSalt, parameterVar],
			Expression.Assign(currentSalt, GenerativeSalts.Peek(generativeSalts)),
			Expression.IfThenElse(
				Expression.Equal(currentSalt, GenerativeSalts.MarkerForNullReference),
				Expression.Assign(parameterVar, BuildNullExpressionFor(type, generativeSalts)),
				Expression.Assign(parameterVar, BuildNonNullInstantiationExpressionFor(type, generativeSalts))),
			parameterVar);
	}

	Expression BuildNullExpressionFor(Type type, ParameterExpression generativeSalts)
	=> type switch
	{
		var t when t.IsValueType && Nullable.GetUnderlyingType(t) is null => Expression.Default(type), // Will not happen during Invoke(), but must not trigger ArgumentExpression during expression tree construction
		var t => Expression.Block(GenerativeSalts.Dequeue(generativeSalts), Expression.Constant(null, t))
	};

	Expression BuildNonNullInstantiationExpressionFor(Type type, ParameterExpression generativeSalts)
	=> ExampleRegenerationBuilder.BuildFor(type, generativeSalts);
}
