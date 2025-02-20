using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.CSharp.RuntimeBinder;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation;

public class Func2
{
	// Func<TInput, TOutput>
	readonly dynamic _funcWithTwoParametersAndAnOutputParameter;

	Func2(dynamic funcWithTwoParametersAndAnOutputParameter)
	{
		_funcWithTwoParametersAndAnOutputParameter = funcWithTwoParametersAndAnOutputParameter;
	}
	public static Func2 CompileCallToStaticMethod(MethodInfo staticMethod)
	=> CompileCallToStaticMethod(staticMethod, staticMethod.GetParameters().Select(p => p.ParameterType).ToArray(), staticMethod.ReturnParameter.ParameterType);

	public static Func2 CompileCallToStaticMethod(MethodInfo staticMethod, Type[] runtimeInputTypes, Type runtimeOutputType)
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
				{ IsVirtual: true } => CompileCallToStaticMethod(BuildDynamicMethodFromVirtualStaticMethod(staticMethod), runtimeInputTypes, runtimeOutputType),
				_ => new(BuildCompiledDelegate(staticMethod, runtimeInputTypes, runtimeOutputType))
			};
		}
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<Func>(staticMethod, runtimeOutputType); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<Func>(defect, staticMethod, runtimeOutputType); }
	}

	public dynamic Invoke(dynamic input, dynamic input2)
	{
		try
		{
			return _funcWithTwoParametersAndAnOutputParameter.Invoke(input, input2);
		}
		catch (RuntimeBinderException ex) when (ex.Message.Contains("MulticastDelegate") && ex.Message.Contains("'Invoke'"))
		{
			var funcArgTypes = ((Type)_funcWithTwoParametersAndAnOutputParameter.GetType()).GetGenericArguments();
			var arg1Type = funcArgTypes[0];
			var arg2Type = funcArgTypes[1];
			var returnType = funcArgTypes[2];
			throw new InvalidOperationException($"{GetType().GetName()}.{nameof(Invoke)}  : Cannot run on types ({arg1Type.FullName}, {arg2Type.FullName}) and return type {returnType.FullName} due to the protection level on one or many of these types.");
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

	static Delegate BuildCompiledDelegate(
		MethodInfo method,
		Type[] runtimeInputTypes,
		Type runtimeOutputType)
	{
		if (runtimeInputTypes.Length != 2) throw new ArgumentException("Expected exactly two input types.", nameof(runtimeInputTypes));
		var methodParameters = method.GetParameters();
		if (methodParameters.Length != 2) throw new ArgumentException("Expected a method with exactly two parameters.", nameof(method));

		var parameter1 = Expression.Parameter(runtimeInputTypes[0], "arg1");
		var parameter2 = Expression.Parameter(runtimeInputTypes[1], "arg2");

		var arg1 = methodParameters[0].ParameterType == runtimeInputTypes[0]
			? (Expression)parameter1
			: Expression.Convert(parameter1, methodParameters[0].ParameterType);

		var arg2 = methodParameters[1].ParameterType == runtimeInputTypes[1]
			? (Expression)parameter2
			: Expression.Convert(parameter2, methodParameters[1].ParameterType);

		var call = Expression.Call(method, arg1, arg2);

		var delegateType = typeof(Func<,,>).MakeGenericType(
			runtimeInputTypes[0],
			runtimeInputTypes[1],
			runtimeOutputType);

		var lambda = Expression.Lambda(
			delegateType,
			call,
			parameter1,
			parameter2);

		return lambda.Compile();
	}
}
