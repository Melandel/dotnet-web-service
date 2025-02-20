using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class ClassWithPublicStaticFactoryMethodExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithPublicStaticFactoryMethodExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, MethodInfo> StaticFactoryMethodWithMostParametersByType = [];
	public static ClassWithPublicStaticFactoryMethodExampleInstantiationBuilder Instance(Type type, MethodInfo[] staticFactoryMethods)
	{
		StaticFactoryMethodWithMostParametersByType.GetOrAdd(type, _ => staticFactoryMethods.OrderByDescending(c => c.GetParameters().Length).ThenBy(c => c.Name).First());
		return _instance;
	}
	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var staticFactoryMethod = StaticFactoryMethodWithMostParametersByType[type];
		var newSaltVariable = Expression.Variable(RandomSalt.Type);
		var newSalt = Expression.Assign(newSaltVariable, RandomSalt.Select);
		return Expression.Block(
			[newSaltVariable],
			Expression.Call(
				null,
				staticFactoryMethod,
				GenerateParameterExamplesFor(staticFactoryMethod, generativeSalts)));
	}
}

