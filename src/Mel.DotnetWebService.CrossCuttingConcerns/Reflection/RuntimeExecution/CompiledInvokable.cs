using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;

public class CompiledInvokable
{
	readonly dynamic _funcOrAction;
	public Type[] ParameterTypes { get; }
	public Type ReturnType { get; }
	CompiledInvokable(object compiledFuncOrAction)
	{
		Type funcOrActionType = compiledFuncOrAction.GetType();
		if (funcOrActionType == typeof(Action))
		{
			ParameterTypes = [];
			ReturnType = typeof(void);
		}

		if (!funcOrActionType.IsGenericType)
		{
			throw ObjectConstructionException.WhenConstructingAnInstanceOf<CompiledInvokable>("Must encapsulate a Func or an Action", funcOrActionType);
		}

		var genericTypeDef = funcOrActionType.GetGenericTypeDefinition();
		if (genericTypeDef.FullName is null)
		{
			throw ObjectConstructionException.WhenConstructingAnInstanceOf<CompiledInvokable>("Must encapsulate be a Func or an Action", funcOrActionType);
		}

		var genericArgtypes = funcOrActionType.GetGenericArguments();
		if (genericTypeDef.FullName.Contains("Func"))
		{
			ParameterTypes = genericArgtypes[..^1];
			ReturnType = genericArgtypes[^1];
		}
		else if (genericTypeDef.FullName.Contains("Action"))
		{
			ParameterTypes = genericArgtypes;
			ReturnType = typeof(void);
		}
		else
		{
			throw ObjectConstructionException.WhenConstructingAnInstanceOf<CompiledInvokable>("Must encapsulate be a Func or an Action", funcOrActionType);
		}

		_funcOrAction = compiledFuncOrAction;
	}

	public static CompiledInvokable FromExpression(Expression methodCallExpression)
	=> FromExpression(Expression.Lambda(methodCallExpression));

	public static CompiledInvokable FromExpression(LambdaExpression lambdaExpression)
	=> FromCompiledFuncOrAction(lambdaExpression.Compile());

	public static CompiledInvokable FromCompiledFuncOrAction(object compiledFuncOrAction)
	=> new(compiledFuncOrAction);

	public dynamic Invoke()
	{
		try { return _funcOrAction.Invoke(); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic input)
	{
		try { return _funcOrAction.Invoke(input); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2)
	{
		try { return _funcOrAction.Invoke(arg1, arg2); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8, dynamic arg9)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8, dynamic arg9, dynamic arg10)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8, dynamic arg9, dynamic arg10, dynamic arg11)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8, dynamic arg9, dynamic arg10, dynamic arg11, dynamic arg12)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8, dynamic arg9, dynamic arg10, dynamic arg11, dynamic arg12, dynamic arg13)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8, dynamic arg9, dynamic arg10, dynamic arg11, dynamic arg12, dynamic arg13, dynamic arg14)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8, dynamic arg9, dynamic arg10, dynamic arg11, dynamic arg12, dynamic arg13, dynamic arg14, dynamic arg15)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	public dynamic Invoke(dynamic arg1, dynamic arg2, dynamic arg3, dynamic arg4, dynamic arg5, dynamic arg6, dynamic arg7, dynamic arg8, dynamic arg9, dynamic arg10, dynamic arg11, dynamic arg12, dynamic arg13, dynamic arg14, dynamic arg15, dynamic arg16)
	{
		try { return _funcOrAction.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16); }
		catch (RuntimeBinderException ex) when (ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(ex)) { throw new InvalidOperationException(RuntimeBinderExceptionMessage); }
		catch (Exception ex) { AddGenericTypesAsSuffixToExceptionMessage(ex); throw; }
	}

	bool ASignatureTypeCouldNotBeResolvedDueToItsProtectionLevel(RuntimeBinderException ex)
	=> ex.Message.Contains("MulticastDelegate") && ex.Message.Contains("'Invoke'");

	string RuntimeBinderExceptionMessage
	{
		get
		{
			var sb = new StringBuilder();
			sb.Append($"{GetType().GetName()}.{nameof(Invoke)}  : Cannot run");
			sb.Append((ParameterTypes, ReturnType) switch
			{
				([], _) => ReturnType switch
				{
				var t when t == typeof(void) => $" the parameterless {nameof(CompiledInvokable)} Action",
				var t => $" the parameterless {ReturnType}-returning {nameof(CompiledInvokable)}",
				},
				(_,_) => $" the {ReturnType.GetName()}-returning {nameof(CompiledInvokable)} taking arguments ({string.Join(", ", ParameterTypes.Select(t => t.FullName))})"
			});
			sb.Append(" due to the protection level on one or many of these types.");
			return sb.ToString();
		}
	}

	void AddGenericTypesAsSuffixToExceptionMessage(Exception ex)
	{
		ex.GetType()
			.GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic)!
			.SetValue(ex, $"{ex.Message}{GenericTypesAsExceptionMessageSuffix}");
	}

	string GenericTypesAsExceptionMessageSuffix
	{
		get
		{
			var sb = new StringBuilder();
			sb.Append(" - Generics: <");

			sb.Append(string.Join(", ", ParameterTypes.Select((parameterType, i) => $"{parameterType.FullName} arg{i + 1}")));
			if (ReturnType != typeof(void))
			{
				sb.Append($", {ReturnType.FullName}");
			}

			sb.Append(">)");
			return sb.ToString();
		}
	}
}
