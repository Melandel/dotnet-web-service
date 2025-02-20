using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class ClassWithPublicConstructorHavingParametersExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithPublicConstructorHavingParametersExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, ConstructorInfo> ConstructorWithMostParametersByType = [];
	public static ClassWithPublicConstructorHavingParametersExampleInstantiationBuilder Instance(Type type, ConstructorInfo[] constructorsWithParameters)
	{
		ConstructorWithMostParametersByType.GetOrAdd(type, _ => constructorsWithParameters.OrderByDescending(c => c.GetParameters().Length).ThenBy(c => c.Name).First());
		return _instance;
	}
	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var constructor = ConstructorWithMostParametersByType[type];
		return Expression.New(constructor, GenerateParameterExamplesFor(constructor, generativeSalts));
	}
}
