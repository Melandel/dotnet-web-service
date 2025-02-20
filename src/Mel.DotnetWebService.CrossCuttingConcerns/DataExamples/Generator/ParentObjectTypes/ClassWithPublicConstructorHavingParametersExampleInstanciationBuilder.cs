using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

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
	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var constructor = ConstructorWithMostParametersByType[type];
		return Expression.New(constructor, GenerateParameterExamplesFor(constructor, currentSalt));
	}
}
