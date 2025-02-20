using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.DynamicallyGeneratedTypes;

class ExceptionUnknownAtCompileTime
{
	public static readonly Type Type;
	public static readonly CompiledInvokable Instantiator;
	static ExceptionUnknownAtCompileTime()
	{
		var typeBuilder = CrossCuttingConcerns.Reflection.DynamicTypeGeneration.DynamicallyGeneratedTypes.DefineType($"{nameof(ExceptionUnknownAtCompileTime)}", TypeAttributes.Public | TypeAttributes.Class, typeof(Exception));
		var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [typeof(string)]);
		var il = constructorBuilder.GetILGenerator();
		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldarg_1);
		il.Emit(OpCodes.Call, typeof(Exception).GetConstructor([typeof(string)]));
		il.Emit(OpCodes.Ret);

		Type = typeBuilder.CreateType();
		var exceptionMessageParameter = Expression.Parameter(typeof(string), "message");
		var instantiation = Expression.New(Type.GetConstructors().First(), exceptionMessageParameter);
		Instantiator = CompiledInvokable.FromExpression(instantiation, exceptionMessageParameter);
	}
}

