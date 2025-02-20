using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;

public class CompiledInvokableBuilder
{
	readonly LambdaExpressionBuilder _lambdaExpressionBuilder;
	CompiledInvokableBuilder(LambdaExpressionBuilder lambdaExpressionBuilder)
	{
		_lambdaExpressionBuilder = lambdaExpressionBuilder;
	}

	public static CompiledInvokableBuilder CreateForParameterlessCompiledInvokable(bool createWithReturnTypeType = true)
	=> CreateForCompiledInvokableHavingParameterTypes([], createWithReturnTypeType);

	public static CompiledInvokableBuilder CreateForCompiledInvokableHavingParameterTypes(IEnumerable<Type> parameterTypes, bool createWithReturnTypeType = true)
	=> new(LambdaExpressionBuilder.CreateWithParameterTypes(parameterTypes, createWithReturnTypeType));

	public CompiledInvokableBuilder AddCallTo(MethodInfo method)
	{
		_lambdaExpressionBuilder.AddCallTo(method);
		return this;
	}

	public CompiledInvokableBuilder AddCallTo(NonEmptyArray<MethodInfo> methods)
	{
		_lambdaExpressionBuilder.AddCallTo(methods);
		return this;
	}

	public CompiledInvokable Build()
	{
		var lambdaExpression = _lambdaExpressionBuilder.Build();
		return CompiledInvokable.FromExpression(lambdaExpression);
	}
}
