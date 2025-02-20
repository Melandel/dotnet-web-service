using System.Reflection;
using System.Reflection.Emit;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ExceptionThrowingStubGeneration;

class ExceptionThrowingStubGeneratorUsingReflectionEmit
{
	public static Type GenerateConcreteTypeImplementing(Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new ArgumentException($"Expected an interface, but got {interfaceType.GetName()}.");
		}

		var typeBuilder = DynamicallyGeneratedTypes.DefineType(
			$"{interfaceType.Name}_ImplementationThatThrows_{Guid.NewGuid():N}",
			TypeAttributes.Public | TypeAttributes.Class);
		typeBuilder.AddInterfaceImplementation(interfaceType);

		var exceptionField = typeBuilder.DefineField("_exceptionToThrow", typeof(Exception), FieldAttributes.Private | FieldAttributes.InitOnly);

		ImplementMethods(typeBuilder, interfaceType, exceptionField);
		ImplementConstructorWithSingleExceptionParameter(typeBuilder, exceptionField);

		var generatedType = typeBuilder.CreateType()!;

		return generatedType;
	}

	static void ImplementMethods(TypeBuilder typeBuilder, Type interfaceType, FieldBuilder exceptionField)
	{
		var parentInterfaces = interfaceType.GetInterfaces();

		var properties = parentInterfaces
			.SelectMany(i => i.GetProperties())
			.Concat(interfaceType.GetProperties())
			.Distinct()
			.ToArray();

		var methods = parentInterfaces
			.SelectMany(i => i.GetMethods())
			.Concat(interfaceType.GetMethods())
			.Distinct()
			.ToArray();
		foreach (var method in methods)
		{
			var parameters = method.GetParameters();

			var methodBuilder = typeBuilder.DefineMethod(
				method.Name,
				MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
				method.ReturnType,
				Array.ConvertAll(parameters, p => p.ParameterType));

			var il = methodBuilder.GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, exceptionField);
			il.Emit(OpCodes.Throw);

			typeBuilder.DefineMethodOverride(methodBuilder, method);
			if (method.ReturnType == typeof(void))
			{
				var matchingSetProperty = properties.FirstOrDefault(p => p.PropertyType == method.ReturnType && p.CanWrite && p.SetMethod!.Name == method.Name);
				if (matchingSetProperty != null)
				{
					var propertyBuilder = typeBuilder.DefineProperty(matchingSetProperty.Name, PropertyAttributes.None, matchingSetProperty.PropertyType, null);
					propertyBuilder.SetSetMethod(methodBuilder);
					typeBuilder.DefineMethodOverride(methodBuilder, matchingSetProperty.SetMethod!);
				}
			}
			else
			{
				var matchingGetProperty = properties.FirstOrDefault(p => p.PropertyType == method.ReturnType && p.CanRead && p.GetMethod!.Name == method.Name);
				if (matchingGetProperty != null)
				{
					var propertyBuilder = typeBuilder.DefineProperty(matchingGetProperty.Name, PropertyAttributes.None, matchingGetProperty.PropertyType, null);
					propertyBuilder.SetGetMethod(methodBuilder);
					typeBuilder.DefineMethodOverride(methodBuilder, matchingGetProperty.GetMethod!);
				}
			}
		}
	}

	static void ImplementConstructorWithSingleExceptionParameter(TypeBuilder typeBuilder, FieldBuilder exceptionField)
	{
		var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [typeof(Exception)]);
		constructor.DefineParameter(1, ParameterAttributes.In, exceptionField.Name.Trim('_'));
		var ctorIL = constructor.GetILGenerator();
		ctorIL.Emit(OpCodes.Ldarg_0);
		ctorIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)!);
		ctorIL.Emit(OpCodes.Ldarg_0);
		ctorIL.Emit(OpCodes.Ldarg_1);
		ctorIL.Emit(OpCodes.Stfld, exceptionField);
		ctorIL.Emit(OpCodes.Ret);
	}
}
