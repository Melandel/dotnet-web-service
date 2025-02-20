using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class ClassWithPublicConstructorHavingParametersExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly ClassWithPublicConstructorHavingParametersExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> ConstructorWithMostParametersByType = [];
	public static ClassWithPublicConstructorHavingParametersExampleInstanciationBuilder Instance(Type type, ConstructorInfo[] constructorsWithParameters)
	{
		if (!ConstructorWithMostParametersByType.ContainsKey(type))
		{
			var constructorWithMostParameters = constructorsWithParameters
				.OrderByDescending(c => c.GetParameters().Length)
				.ThenBy(c => c.Name)
				.First();
			ConstructorWithMostParametersByType.Add(type, constructorWithMostParameters);
		}
		return _instance;
	}
	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var constructor = ConstructorWithMostParametersByType[type];
		return Expression.New(constructor, GenerateParameterExamplesFor(constructor, generativeSalts));
	}
}
