using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class DelegateExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	record ParameterExpressionsAndReturnType(ParameterExpression[] ParameterExpressions, Type ReturnType);
	static readonly Dictionary<Type, ParameterExpressionsAndReturnType> ExpressionParametersByDelegateType = [];
	public static readonly DelegateExampleInstanciationBuilder Instance = new();

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		if (!ExpressionParametersByDelegateType.TryGetValue(type, out var parameterExpressionsAndReturnType))
		{
			parameterExpressionsAndReturnType = GetParameterExpressionsAndReturnType(type);
			ExpressionParametersByDelegateType.Add(type, parameterExpressionsAndReturnType);
		}

		return Expression.Lambda(
			type,
			ExampleInstanciationBuilder.BuildFor(parameterExpressionsAndReturnType.ReturnType, currentSalt),
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
