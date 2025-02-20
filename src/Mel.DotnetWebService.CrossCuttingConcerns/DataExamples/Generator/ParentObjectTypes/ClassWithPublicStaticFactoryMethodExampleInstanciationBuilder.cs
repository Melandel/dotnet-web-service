using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithPublicStaticFactoryMethodExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithPublicStaticFactoryMethodExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, MethodInfo> StaticFactoryMethodWithMostParametersByType = [];
	public static ClassWithPublicStaticFactoryMethodExampleInstantiationBuilder Instance(Type type, MethodInfo[] staticFactoryMethods)
	{
		StaticFactoryMethodWithMostParametersByType.GetOrAdd(
			type,
			_ => staticFactoryMethods
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First());
		return _instance;
	}
	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var staticFactoryMethod = StaticFactoryMethodWithMostParametersByType[type];
		var newSalt = currentSalt;
		return Expression.Call(
			null,
			staticFactoryMethod,
			GenerateParameterExamplesFor(staticFactoryMethod, newSalt, parentObjectsAlreadyRecursedThrough));
	}
}

