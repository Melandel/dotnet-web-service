using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithConstructorHavingParametersExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithConstructorHavingParametersExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, ConstructorInfo> ConstructorWithMostParametersByType = [];
	public static ClassWithConstructorHavingParametersExampleInstantiationBuilder Instance(Type type, ConstructorInfo[] constructorsWithParameters)
	{
		ConstructorWithMostParametersByType.GetOrAdd(
			type,
			_ => constructorsWithParameters
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First());
		return _instance;
	}
	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var constructor = ConstructorWithMostParametersByType[type];
		return Expression.New(constructor, GenerateParameterExamplesFor(constructor, currentSalt, parentObjectsAlreadyRecursedThrough));
	}
}
