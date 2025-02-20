using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class DelegateExampleInstantiationBuilder : ExampleInstantiationBuilder
{
	record ParameterExpressionsAndReturnType(ParameterExpression[] ParameterExpressions, Type ReturnType);
	static readonly ConcurrentDictionary<Type, ParameterExpressionsAndReturnType> ExpressionParametersByDelegateType = [];
	public static readonly DelegateExampleInstantiationBuilder Instance = new();

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		if (!ExpressionParametersByDelegateType.TryGetValue(type, out var parameterExpressionsAndReturnType))
		{
			parameterExpressionsAndReturnType = ExpressionParametersByDelegateType.GetOrAdd(type, _ => GetParameterExpressionsAndReturnType(type));
		}

		return Expression.Lambda(
			type,
			ExampleInstantiationBuilder.BuildFor(parameterExpressionsAndReturnType.ReturnType, currentSalt, parentObjectsAlreadyRecursedThrough),
			parameterExpressionsAndReturnType.ParameterExpressions);
	}

	static ParameterExpressionsAndReturnType GetParameterExpressionsAndReturnType(Type type)
	{
		var invoke = type.GetMethod("Invoke")!;
		var parameters = invoke.GetParameters();
		var parameterExpressions = invoke.GetParameters()
			.Select(p => Expression.Parameter(p.ParameterType, p.Name))
			.ToArray();

		var argTypes = type.GetGenericArguments();
		var returnType = parameters.Length == argTypes.Length
			? typeof(void)
			: argTypes[^1];
		return new(parameterExpressions, returnType);
	}
}
