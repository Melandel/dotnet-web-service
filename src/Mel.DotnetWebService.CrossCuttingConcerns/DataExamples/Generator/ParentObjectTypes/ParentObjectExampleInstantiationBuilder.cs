using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

abstract class ParentObjectExampleInstantiationBuilder : ExampleInstantiationBuilder
{
	static readonly ConcurrentDictionary<Type, ParentObjectExampleInstantiationBuilder> ParentObjectGeneratorsByType = [];

	public static ParentObjectExampleInstantiationBuilder InstanceSuitedFor(Type type)
	=> ParentObjectGeneratorsByType.GetOrAdd(type, _ => type switch
	{
		var t when ConstrainedTypeInfos.Include(t) => ConstrainedTypeExampleInstantiationBuilder.Instance,
		var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor) => PositionalRecordParentObjectExampleInstantiationBuilder.Instance(type, propertyBasedConstructor!),
		var t when t.IsClassWithPublicConstructorHavingParameters(out var constructorsWithParameters) => ClassWithConstructorHavingParametersExampleInstantiationBuilder.Instance(type, constructorsWithParameters),
		var t when t.IsClassWithPublicParameterlessConstructorAndPublicSetters(out var publicSettableProperties) => ClassWithPublicParameterlessConstructorAndPublicSettersExampleInstantiationBuilder.Instance(type, publicSettableProperties),
		var t when t.IsClassWithPublicStaticFactoryMethod(out var staticFactoryMethods) => ClassWithPublicStaticFactoryMethodExampleInstantiationBuilder.Instance(type, staticFactoryMethods),
		var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(out var singletonAccessProperty) => ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyExampleInstantiationBuilder.Instance(type, singletonAccessProperty!),
		var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughField(out var singletonAccessProperty) => ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstantiationBuilder.Instance(type, singletonAccessProperty!),
		var t when t.IsClassWithPublicInstancesExposedAsStaticReadonlyFields(out var staticReadonlyInstanceAccessFields) => ClassWithPublicInstancesExposedAsFieldsExampleInstantiationBuilder.Instance(type, staticReadonlyInstanceAccessFields),
		var t when t.IsClassWithPublicInstancesExposedAsProperties(out var staticInstanceAccessProperties) => ClassWithPublicInstancesExposedAsPropertiesExampleInstantiationBuilder.Instance(type, staticInstanceAccessProperties),
		var t when t.IsClassWithPublicInstancesExposedAsStaticFields(out var staticInstanceAccessFields) => ClassWithPublicInstancesExposedAsFieldsExampleInstantiationBuilder.Instance(type, staticInstanceAccessFields),
		var t when t.IsClassWithPrivateConstructorHavingParameters(out var constructorsWithParameters) => ClassWithConstructorHavingParametersExampleInstantiationBuilder.Instance(type, constructorsWithParameters),
		_ => throw new InvalidOperationException($"{nameof(ParentObjectExampleInstantiationBuilder)}: Cannot resolve an {nameof(ExampleValueGeneratorUsingExpressionTrees)} for type {type.GetName()}.")
	});

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		if (!parentObjectsAlreadyRecursedThrough.Add(type))
		{
			return Expression.Constant(null, type);
		}

		var instantiationExpression = BuildParentObjectInstantiationExpressionFor(type, currentSalt, parentObjectsAlreadyRecursedThrough);

		parentObjectsAlreadyRecursedThrough.Remove(type);
		return instantiationExpression;
	}

	public abstract Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough);

	protected Expression[] GenerateParameterExamplesFor(MethodBase constructorOrMethod, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	=> constructorOrMethod.GetParameters() switch
	{
		[] => [],
		[var singleParameter] => GenerateSingleParameter(singleParameter, currentSalt, parentObjectsAlreadyRecursedThrough),
		var multipleParameters => GenerateMultipleParameters(ArrayWithAtLeast2Items.ApplyConstraintsTo(multipleParameters), currentSalt, parentObjectsAlreadyRecursedThrough)
	};

	Expression[] GenerateSingleParameter(ParameterInfo singleConstructionParameter, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var newSalt = currentSalt;
		var parameter = ExampleInstantiationBuilder.BuildFor(
			singleConstructionParameter.ParameterType,
			newSalt,
			parentObjectsAlreadyRecursedThrough);
		return [parameter];
	}
	Expression[] GenerateMultipleParameters(ArrayWithAtLeast2Items<ParameterInfo> constructionParameters, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var parameters = new List<Expression>(constructionParameters.Length);
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
			var newSalt = Expression.Add(currentSalt, Expression.Constant(saltOffset));
			var parameterExampleValue = ExampleInstantiationBuilder.BuildFor(p.ParameterType, newSalt, parentObjectsAlreadyRecursedThrough);
			parameters.Add(parameterExampleValue);
		}

		return parameters.ToArray();
	}
}
