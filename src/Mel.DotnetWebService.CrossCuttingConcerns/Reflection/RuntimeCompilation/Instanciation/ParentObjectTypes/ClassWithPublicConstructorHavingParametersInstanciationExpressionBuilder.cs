using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class ClassWithPublicConstructorHavingParametersInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly ClassWithPublicConstructorHavingParametersInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> ConstructorWithMostParametersByType = [];
	public static ClassWithPublicConstructorHavingParametersInstanciationExpressionBuilder Instance(Type type, ConstructorInfo[] constructorsWithParameters)
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
	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		var constructor = ConstructorWithMostParametersByType[type];
		var arguments = constructor
			.GetParameters()
			.Select(p =>
			InstanciationExpressionBuilder.BuildFor(
				p.ParameterType,
				recursionStack,
				salt))
			.ToArray();

		return Expression.New(constructor, arguments);
		// var constructor = ConstructorWithMostParametersByType[type];
		// var parameters = new List<object>();
		// var saltOffsetByType = new Dictionary<Type, int>();
		// foreach (var p in constructor.GetParameters())
		// {
		// 	if (saltOffsetByType.TryGetValue(p.ParameterType, out var currentSaltOffsetForThisType))
		// 	{
		// 		saltOffsetByType[p.ParameterType] = ++currentSaltOffsetForThisType;
		// 	}
		// 	else
		// 	{
		// 		saltOffsetByType.Add(p.ParameterType, 0);
		// 	}
		// 	var saltOffset = saltOffsetByType[p.ParameterType];
		// 	var parameterExampleValue = ExampleValueInstanciationExpressionBuilder.GenerateExampleOf(p.ParameterType, salt+saltOffset);
		// 	parameters.Add(parameterExampleValue);
		// }

		// var instance = constructor.Invoke(parameters.ToArray());
		// return instance;
	}
}
