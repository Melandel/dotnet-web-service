using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class ClassWithPublicStaticFactoryMethodInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly ClassWithPublicStaticFactoryMethodInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, MethodInfo> StaticFactoryMethodWithMostParametersByType = [];
	public static ClassWithPublicStaticFactoryMethodInstanciationExpressionBuilder Instance(Type type, MethodInfo[] staticFactoryMethods)
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
	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// var staticFactoryMethod = StaticFactoryMethodWithMostParametersByType[type];
		// var parameters = new List<object>();
		// var saltOffsetByType = new Dictionary<Type, int>();
		// foreach (var p in staticFactoryMethod.GetParameters())
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
		// 	var parameterExampleValue = InstanciationExpressionBuilder.BuildInstanciationExpressionFor(p.ParameterType, salt+saltOffset);
		// 	parameters.Add(parameterExampleValue);
		// }

		// var instance = staticFactoryMethod.Invoke(null, parameters.ToArray())!;
		// return instance;
	}
}

