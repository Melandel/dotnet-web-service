using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.CSharp.RuntimeBinder;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation;

public class Func
{
	// Func<TInput, TOutput>
	readonly dynamic _funcWithSingleParameterAndAnOutputParameter;

	Func(dynamic funcWithSingleParameterAndAnOutputParameter)
	{
		_funcWithSingleParameterAndAnOutputParameter = funcWithSingleParameterAndAnOutputParameter;
	}
	public static Func CompileCallToStaticMethod(MethodInfo staticMethod)
	=> CompileCallToStaticMethod(staticMethod, staticMethod.GetParameters().Single().ParameterType, staticMethod.ReturnParameter.ParameterType);

	public static Func CompileCallToStaticMethod(MethodInfo staticMethod, Type runtimeInputType, Type runtimeOutputType)
	{
		try
		{
			if (runtimeOutputType.IsInterface || runtimeOutputType.IsAbstract)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<Func>($"the method must be owned by a {nameof(Type.DeclaringType)} that can be instantiated");
			}

			return staticMethod switch
			{
				{ IsStatic: false } => throw ObjectConstructionException.WhenConstructingAnInstanceOf<Func>("the method must be static"),
				// 👇 A DynamicMethod is built and used instead of the raw MethodInfo
				//   Justification: A BadImageFormatException is thrown when invoking static abstract method through reflection at the time of writing (2026/02/15)
				//   See https://github.com/dotnet/runtime/issues/79331
				{ IsVirtual: true } => CompileCallToStaticMethod(BuildDynamicMethodFromVirtualStaticMethod(staticMethod), runtimeInputType, runtimeOutputType),
				_ => new(BuildCompiledDelegate(staticMethod, runtimeInputType, runtimeOutputType))
			};
		}
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<Func>(staticMethod, runtimeOutputType); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<Func>(defect, staticMethod, runtimeOutputType); }
	}

	public static Func CompileSuccessiveCallsToStaticMethods(IEnumerable<MethodInfo> successionOfStaticMethods, Type runtimeInputType, Type runtimeOutputType)
	{
		try
		{
			if (runtimeOutputType.IsInterface || runtimeOutputType.IsAbstract)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<Func>($"the method must be owned by a {nameof(Type.DeclaringType)} that can be instantiated");
			}

			return new(BuildCompiledDelegate(successionOfStaticMethods.ToArray(), runtimeInputType, runtimeOutputType));
		}
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<Func>(successionOfStaticMethods, runtimeOutputType); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<Func>(defect, successionOfStaticMethods, runtimeOutputType); }
	}
	public dynamic Invoke(dynamic input)
	{
		try
		{
			return _funcWithSingleParameterAndAnOutputParameter.Invoke(input);
		}
		catch (RuntimeBinderException ex) when (ex.Message.Contains("MulticastDelegate") && ex.Message.Contains("'Invoke'"))
		{
			var funcArgTypes = ((Type)_funcWithSingleParameterAndAnOutputParameter.GetType()).GetGenericArguments();
			var arg1Type = funcArgTypes[0];
			var returnType = funcArgTypes[1];
			throw new InvalidOperationException($"{GetType().GetName()}.{nameof(Invoke)}  : Cannot run on type {arg1Type.FullName} and return type {returnType.FullName} due to the protection level on one or many of these types.");
		}
		catch (Exception ex)
		{
			ex.GetType()
				.GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic)!
				.SetValue(ex, $"{ex.Message} ({nameof(input)} being a {input.GetType()})");
			throw;
		}
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

	static dynamic BuildCompiledDelegate(MethodInfo method, Type runtimeInputType, Type runtimeOutputType)
	{
		var typedDelegateCreationOperation = typeof(Expression)
			.GetMethods()
			.First(mi => mi.Name == nameof(Expression.Lambda) && mi.IsGenericMethod && mi.GetParameters().Length == 2)
			.MakeGenericMethod(typeof(Func<,>)
			.MakeGenericType(runtimeInputType, runtimeOutputType));

		var parameterExpression = Expression.Parameter(runtimeInputType);
		var methodParameterType = method.GetParameters()[0].ParameterType;
		dynamic typedDelegate = typedDelegateCreationOperation
			.Invoke(
				null,
				new object[]
				{
					Expression.Call(
						method,
						methodParameterType == runtimeInputType
							? parameterExpression
							: Expression.Convert(parameterExpression, methodParameterType)),
					new ParameterExpression[] { parameterExpression }
				})!;
		var compiledTypedDelegate = typedDelegate.Compile();
		return compiledTypedDelegate;
	}

	static dynamic BuildCompiledDelegate(MethodInfo[] successionOfMethods, Type runtimeInputType, Type runtimeOutputType)
	{
		var typedDelegateCreationOperation = typeof(Expression)
			.GetMethods()
			.First(mi => mi.Name == nameof(Expression.Lambda) && mi.IsGenericMethod && mi.GetParameters().Length == 2)
			.MakeGenericMethod(typeof(Func<,>)
			.MakeGenericType(runtimeInputType, runtimeOutputType));

		var parameterExpression = Expression.Parameter(runtimeInputType, "input");
		var parameterExpressions = new List<ParameterExpression>() { parameterExpression };
		MethodCallExpression? body = null;
		Type? lastReturnType = null;
		var variables = new List<ParameterExpression>() { parameterExpression };
		var expressions = new List<Expression>();
		var lastMethodIndex = successionOfMethods.Length-1;
		for (var i = 0; i < successionOfMethods.Length; i++)
		{
			var method = successionOfMethods[i];
			if (method is null)
			{
				continue;
			}
			var methodParameterType = method.GetParameters()[0].ParameterType;
			Expression chosenMethodParameter =
				body == null
					? methodParameterType == runtimeInputType
						? parameterExpression
						: Expression.Convert(parameterExpression, methodParameterType)
					: methodParameterType == lastReturnType
						? body!
						: Expression.Convert(body!, methodParameterType);

			body = Expression.Call(method, chosenMethodParameter);
			lastReturnType = method.ReturnType;
		}

		dynamic typedDelegate = typedDelegateCreationOperation
		.Invoke(
			null,
			new object[]
			{
					body!,
					new ParameterExpression[] { parameterExpression }
			})!;
		var compiledTypedDelegate = typedDelegate.Compile();
		return compiledTypedDelegate;
	}
}
