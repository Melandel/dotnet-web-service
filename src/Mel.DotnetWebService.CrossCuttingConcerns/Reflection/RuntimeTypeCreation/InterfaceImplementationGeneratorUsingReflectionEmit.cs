using System.Reflection;
using System.Reflection.Emit;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeCreation;

static class InterfaceImplementationGeneratorUsingReflectionEmit
{
	public const string DynamicallyGeneratedTypesAssembly  = nameof(DynamicallyGeneratedTypesAssembly);
	static readonly Dictionary<Type, MethodInfo> HardCodedValueInstanciatorsByType = [];

	public static Type GenerateConcreteTypeImplementing(Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new ArgumentException($"Expected an interface, but got {interfaceType.GetName()}.");
		}
		var typeBuilder = InterfaceImplementionGenerator.Module.DefineType($"{interfaceType.Name}_FieldBasedImplementation_{Guid.NewGuid():N}", TypeAttributes.Public | TypeAttributes.Class);
		typeBuilder.AddInterfaceImplementation(interfaceType);

		var fields = new List<FieldBuilder>();
		ImplementMethods(typeBuilder, interfaceType, fields);
		ImplementConstructorWithOneParameterForEachField(typeBuilder, fields);

		var type = typeBuilder.CreateType();
		return type;
	}

	static void ImplementConstructorWithOneParameterForEachField(TypeBuilder typeBuilder, List<FieldBuilder> fields)
	{
		var parameterTypes = fields.Select(f => f.FieldType).ToArray();
		var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);
		for (int i = 0; i < parameterTypes.Length; i++)
		{
			constructor.DefineParameter(i+1, ParameterAttributes.In, fields[i].Name.Trim('_'));
		}
		var il = constructor.GetILGenerator();
		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)!);
		for (var i = 0; i < fields.Count; i++)
		{
			var field = fields[i];
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg, i+1);
			il.Emit(OpCodes.Stfld, field);
		}
		il.Emit(OpCodes.Ret);
	}

	static void ImplementMethods(TypeBuilder typeBuilder, Type interfaceType, List<FieldBuilder> fields)
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
			if (method.ReturnType == typeof(void))
			{
				il.Emit(OpCodes.Ret);

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
					var field = DefineReadonlyField(method.ReturnType, $"_{matchingGetProperty.Name.WithFirstCharacterInLowerCase()}", typeBuilder);

					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Ldfld, field);
					il.Emit(OpCodes.Ret);
					fields.Add(field);

					var propertyBuilder = typeBuilder.DefineProperty(matchingGetProperty.Name, PropertyAttributes.None, matchingGetProperty.PropertyType, null);
					propertyBuilder.SetGetMethod(methodBuilder);
					typeBuilder.DefineMethodOverride(methodBuilder, matchingGetProperty.GetMethod!);
				}
				else
				{
					if (!HardCodedValueInstanciatorsByType.TryGetValue(method.ReturnType, out var methodInfo))
					{
						methodInfo = typeof(ExampleValueGenerator)
							.GetMethods(BindingFlags.Public | BindingFlags.Static)
							.Where(m => m.Name == nameof(ExampleValueGenerator.GenerateExampleOf))
							.Where(m => m.IsGenericMethod && m.GetGenericArguments().Length == 1)
							.Where(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(int))
							.Single()
							.MakeGenericMethod(method.ReturnType);
						HardCodedValueInstanciatorsByType.Add(method.ReturnType, methodInfo);
					}

					il.Emit(OpCodes.Ldc_I4_0);
					il.Emit(OpCodes.Call, methodInfo);
					il.Emit(OpCodes.Ret);
				}
			}

			typeBuilder.DefineMethodOverride(methodBuilder, method);
		}
	}

	static FieldBuilder DefineReadonlyField(Type fieldType, string fieldName, TypeBuilder typeBuilder)
	=> typeBuilder.DefineField(fieldName, fieldType, FieldAttributes.Private | FieldAttributes.InitOnly);
}
