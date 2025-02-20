using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class ClassWithPublicStaticFactoryMethodExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly ClassWithPublicStaticFactoryMethodExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, MethodInfo> StaticFactoryMethodWithMostParametersByType = [];
	public static ClassWithPublicStaticFactoryMethodExampleInstanciationBuilder Instance(Type type, MethodInfo[] staticFactoryMethods)
	{
		if (!StaticFactoryMethodWithMostParametersByType.ContainsKey(type))
		{
			var staticFactoryMethodWithMostParameters = staticFactoryMethods
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First();
			StaticFactoryMethodWithMostParametersByType.Add(type, staticFactoryMethodWithMostParameters);
		}
		return _instance;
	}
	public override Expression BuildInstanciationExpressionFor(Type type, ParameterExpression generativeSalts)
	{
		var staticFactoryMethod = StaticFactoryMethodWithMostParametersByType[type];
		return Expression.Call(
			null,
			staticFactoryMethod,
			GenerateParameterExamplesFor(staticFactoryMethod, generativeSalts));
	}
}

