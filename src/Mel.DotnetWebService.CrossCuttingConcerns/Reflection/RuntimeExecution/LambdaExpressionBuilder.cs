using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;

class LambdaExpressionBuilder
{
	enum LambdaExpressionCategory
	{
		TechnicalDefaultEnumValue = 0,
		Func = 1,
		Action = 2
	};
	readonly List<MethodInfo> _methodsToCallSuccessively = [];
	readonly Type[] _parameterTypes;
	readonly LambdaExpressionCategory _lambdaExpressionCategory;

	LambdaExpressionBuilder(Type[] parameterTypes, LambdaExpressionCategory lambdaExpressionCategory)
	{
		_parameterTypes = parameterTypes;
		_lambdaExpressionCategory = lambdaExpressionCategory;
	}

	public static LambdaExpressionBuilder CreateWithParameterTypes(IEnumerable<Type> parameterTypes, bool createWithReturnTypeType = true)
	=> new(parameterTypes.ToArray(), createWithReturnTypeType ? LambdaExpressionCategory.Func : LambdaExpressionCategory.Action);

	public LambdaExpressionBuilder AddCallTo(MethodInfo method)
	{
		// 👇 A DynamicMethod is built and used instead of the raw MethodInfo
		//   Justification: A BadImageFormatException is thrown when invoking static abstract method through reflection at the time of writing (2026/02/15)
		//   See https://github.com/dotnet/runtime/issues/79331
		var methodThatDoesNotThrowBadImageFormatExceptionWhenInvoked = method switch
		{
			{ IsStatic: true, IsVirtual: true } => BuildDynamicMethodFromVirtualStaticMethod(method),
			_ => method
		};
		_methodsToCallSuccessively.Add(methodThatDoesNotThrowBadImageFormatExceptionWhenInvoked);
		return this;
	}

	public LambdaExpressionBuilder AddCallTo(NonEmptyArray<MethodInfo> methods)
	{
		foreach (var method in methods)
		{
			AddCallTo(method);
		}
		return this;
	}

	public LambdaExpression Build()
	{
		var paramExpressions = _parameterTypes
			.Select((type,index) => Expression.Parameter(type, $"arg{index}"))
			.ToArray();

		return (LambdaExpression) LambdaExpressionCreationMethod
			.Invoke(
				null,
				new object[] {
					BuildSuccessiveChainedCallsToMethod(paramExpressions),
					paramExpressions
					})!;
	}

	public dynamic BuildAsDynamic()
	=> Build();

	MethodCallExpression BuildSuccessiveChainedCallsToMethod(ParameterExpression[] paramExpressions)
	{
		MethodCallExpression? body = null;
		Type? lastReturnType = null;
		var successionOfMethods = _methodsToCallSuccessively.ToArray();
		for (var i = 0; i < successionOfMethods.Length; i++)
		{
			var method = successionOfMethods[i];
			if (method is null) { continue; }

			if (body is null)
			{
				var parameterExpressions = BuildParameterExpressions(paramExpressions, method.GetParameters());
				body = Expression.Call(method, parameterExpressions);
			}
			else
			{
				var methodParameterType = method.GetParameters()[0].ParameterType;
				body = Expression.Call(method, ConvertIfNecessary(body, methodParameterType));
			}
			lastReturnType = method.ReturnType;
		}

		return body!;
	}

	Expression[] BuildParameterExpressions(ParameterExpression[] paramExpressions, ParameterInfo[] methodParameters)
	{
		var parameterExpressions = new Expression[methodParameters.Length];
		for (var i = 0; i < methodParameters.Length; i++)
		{
			var signatureParameterType = methodParameters[i].ParameterType;
			var parameterExpression = ConvertIfNecessary(paramExpressions[i], signatureParameterType);
			parameterExpressions[i] = parameterExpression;
		}

		return parameterExpressions;
	}

	Expression ConvertIfNecessary(Expression expression, Type targetType)
	{
		if (expression.Type == targetType || targetType.IsAssignableFrom(expression.Type))
		{
			return expression;
		}
		return Expression.Convert(expression, targetType);
	}

	static DynamicMethod BuildDynamicMethodFromVirtualStaticMethod(MethodInfo method)
	{
		Type[] parameterTypes = [.. method.GetParameters().Select(p => p.ParameterType)];
		DynamicMethod dynamicMethod = new($"{method.Name}_", method.ReturnType, parameterTypes);
		ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
		for (int i = 0; i < parameterTypes.Length; i++)
		{
			switch (i)
			{
				case 0:
					ilGenerator.Emit(OpCodes.Ldarg_0);
					break;
				case 1:
					ilGenerator.Emit(OpCodes.Ldarg_1);
					break;
				case 2:
					ilGenerator.Emit(OpCodes.Ldarg_2);
					break;
				case 3:
					ilGenerator.Emit(OpCodes.Ldarg_3);
					break;
				case > 3 and <= 255:
					ilGenerator.Emit(OpCodes.Ldarg_S, (byte)i);
					break;
				default:
					ilGenerator.Emit(OpCodes.Ldarg, i);
					break;
			}
		}
		ilGenerator.Emit(OpCodes.Constrained, method.ReturnType);
		ilGenerator.Emit(OpCodes.Call, method);
		ilGenerator.Emit(OpCodes.Ret);
		return dynamicMethod;
	}

	Type GetFuncType(int numberOfGenericParameters)
	=> numberOfGenericParameters switch
	{
		0  => typeof(Func<>),
		1  => typeof(Func<,>),
		2  => typeof(Func<,,>),
		3  => typeof(Func<,,,>),
		4  => typeof(Func<,,,,>),
		5  => typeof(Func<,,,,,>),
		6  => typeof(Func<,,,,,,>),
		7  => typeof(Func<,,,,,,,>),
		8  => typeof(Func<,,,,,,,,>),
		9  => typeof(Func<,,,,,,,,,>),
		10 => typeof(Func<,,,,,,,,,,>),
		11 => typeof(Func<,,,,,,,,,,,>),
		12 => typeof(Func<,,,,,,,,,,,,>),
		13 => typeof(Func<,,,,,,,,,,,,,>),
		14 => typeof(Func<,,,,,,,,,,,,,,>),
		15 => typeof(Func<,,,,,,,,,,,,,,,>),
		16 => typeof(Func<,,,,,,,,,,,,,,,,>),
		_  => throw new InvalidOperationException($"{typeof(Func<>)} can only have between 0 and 16 parameters")
	};

	Type GetActionType(int numberOfGenericParameters)
	=> numberOfGenericParameters switch
	{
		0 => typeof(Action),
		1  => typeof(Action<>),
		2  => typeof(Action<,>),
		3  => typeof(Action<,,>),
		4  => typeof(Action<,,,>),
		5  => typeof(Action<,,,,>),
		6  => typeof(Action<,,,,,>),
		7  => typeof(Action<,,,,,,>),
		8  => typeof(Action<,,,,,,,>),
		9  => typeof(Action<,,,,,,,,>),
		10 => typeof(Action<,,,,,,,,,>),
		11 => typeof(Action<,,,,,,,,,,>),
		12 => typeof(Action<,,,,,,,,,,,>),
		13 => typeof(Action<,,,,,,,,,,,,>),
		14 => typeof(Action<,,,,,,,,,,,,,>),
		15 => typeof(Action<,,,,,,,,,,,,,,>),
		16 => typeof(Action<,,,,,,,,,,,,,,,>),
		_  => throw new InvalidOperationException($"{typeof(Action<>)} can only have between 0 and 16 parameters")
	};

	MethodInfo LambdaExpressionCreationMethod
	=> _lambdaExpressionCategory switch
	{
		LambdaExpressionCategory.Func => FuncCreationMethod,
		LambdaExpressionCategory.Action => ActionCreationMethod,
		_ => throw new NotImplementedException()
	};

	MethodInfo FuncCreationMethod
	{
		get
		{
			var returnType = _methodsToCallSuccessively.Last().ReturnParameter.ParameterType;
			var genericFuncTypeArguments = _parameterTypes.Append(returnType).ToArray();
			var funcType = GetFuncType(_parameterTypes.Length).MakeGenericType(genericFuncTypeArguments);
			return GetLambdaExpressionCreationOperation(funcType);
		}
	}

	MethodInfo ActionCreationMethod
	{
		get
		{
			var actionType = _parameterTypes.Length == 0
				? typeof(Action)
				: GetActionType(_parameterTypes.Length).MakeGenericType(_parameterTypes.ToArray());
			return GetLambdaExpressionCreationOperation(actionType);
		}
	}

	MethodInfo GetLambdaExpressionCreationOperation(Type funcOrActionType)
	=> typeof(Expression)
		.GetMethods()
		.First(mi => mi.Name == nameof(Expression.Lambda) && mi.IsGenericMethod && mi.GetParameters().Length == 2)
		.MakeGenericMethod(funcOrActionType);
}
