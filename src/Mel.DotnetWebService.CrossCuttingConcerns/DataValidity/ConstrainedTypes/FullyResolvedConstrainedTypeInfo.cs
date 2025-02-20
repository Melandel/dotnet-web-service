using System.Collections;
using System.Formats.Asn1;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedTypeInfo
{
	public Type Type { get; }
	public NonEmptyArray<Type> RootTypes { get; }
	public Type RootType => RootTypes.Last();
	public Type FullyNativeRootType => BuildFullNativeTypeFrom(RootTypes.Last());
	Type BuildFullNativeTypeFrom(Type type)
	{
		if (type == typeof(string))
		{
			return typeof(string);
		}

		if (ConstrainedTypeInfos.TryGet(type, out var constrainedTypeInfo))
		{
			return BuildFullNativeTypeFrom(constrainedTypeInfo.RootType);
		}

		if (!type.IsOrInvolvesAConstrainedType())
		{
			return type;
		}

		if (type.IsArray)
		{
			return BuildFullNativeTypeFrom(type.GetElementType()!).MakeArrayType();
		}

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
		{
			var kvpTypes = type.GetGenericArguments();
			return typeof(KeyValuePair<,>).MakeGenericType(kvpTypes.Select(BuildFullNativeTypeFrom).ToArray());
		}

		if (type.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes))
		{
			if (type.IsGenericType)
			{
				return BuildFullNativeTypeFrom(type.GetGenericTypeDefinition()).MakeGenericType(type.GetGenericArguments().Select(BuildFullNativeTypeFrom).ToArray());
			}

			var itemType = argTypes.Single();
			return typeof(IEnumerable<>).MakeGenericType([ BuildFullNativeTypeFrom(itemType) ]);
		}

		return type;
	}
	//=> RootTypes.Last();
	//public Type NativeRootType => RootTypes.Last();
	//RootType
	public MethodInfo InstanciationMethod { get; internal set; }
	public MethodInfo InvokableInstanciationMethod
	=> MakeInvokable(InstanciationMethod);
	public LambdaExpression ConvertorToNativeRootTypeExpression { get; }
	readonly Lazy<CompiledInvokable> _instanciator;
	readonly Lazy<CompiledInvokable> _convertorToNativeRootType;
	readonly Lazy<object> _typeExampleValues;

	public dynamic InvokeStaticFactoryMethod(dynamic valueAsNativeRootType)
	=> _instanciator.Value.Invoke(valueAsNativeRootType);

	public dynamic InvokeImplicitConversionToRootType(dynamic valueAsFullyResolvedConstrainedType)
	=> _convertorToNativeRootType.Value.Invoke(valueAsFullyResolvedConstrainedType);

	public dynamic InvokeImplicitConversionToFullyNativeType(dynamic valueAsFullyResolvedConstrainedType)
	{
		var asRootType = _convertorToNativeRootType.Value.Invoke(valueAsFullyResolvedConstrainedType);
		if (!RootType.IsOrInvolvesAConstrainedType())
		{
			return asRootType;
		}

		if (ConstrainedTypeInfos.TryGet(RootType, out var constrainedTypeInfo))
		{
			return constrainedTypeInfo.InvokeImplicitConversionToFullyNativeType(asRootType);
		}

		if (RootType.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes))
		{
			return CollectionConverter.Convert(asRootType, FullyNativeRootType);
		}

		throw new InvalidOperationException();
	}

	public IList ValidValueExamples
	//public IList ValidNativeRootTypeValueExamples
		//ValidValueExamples
	=> (dynamic) _typeExampleValues.Value
		.GetType()
		.GetProperty(nameof(ExampleValues<object>.ValidValues))!
		.GetValue(_typeExampleValues.Value)!;

	public IReadOnlyCollection<ConstraintViolationExample> ErrorMessagesByInvalidNativeRootTypeValueExample
	=> (dynamic) _typeExampleValues.Value
		.GetType()
		.GetProperty(nameof(ExampleValues<object>.ConstraintViolationExamples))!
		.GetValue(_typeExampleValues.Value)!;

	protected ConstrainedTypeInfo(
		Type type,
		NonEmptyArray<Type> rootTypes,
		MethodInfo instanciationMethod,
		LambdaExpression convertorToNativeRootTypeExpression)
	{
		Type = type;
		RootTypes = rootTypes;
		InstanciationMethod = instanciationMethod;
		_instanciator = new Lazy<CompiledInvokable>(() => CompiledInvokable.FromMethodInfo(instanciationMethod));
		ConvertorToNativeRootTypeExpression = convertorToNativeRootTypeExpression;
		_convertorToNativeRootType = new Lazy<CompiledInvokable>(() => CompiledInvokable.FromExpression(ConvertorToNativeRootTypeExpression));
		_typeExampleValues = new Lazy<object>(
			() => Type
				.GetProperty(
					nameof(IConstrainedValue<int, ConstrainedInt>.Examples),
					BindingFlags.Static | BindingFlags.Public)!
				.GetValue(null, null)!);
	}

	static MethodInfo MakeInvokable(MethodInfo method)
	{
		if (!method.IsStatic)
		{
			return method;
		}

		if (!method.IsVirtual)
		{
			return method;
		}

		// 👇 A DynamicMethod is built and used instead of the raw MethodInfo
		//   Justification: A BadImageFormatException is thrown when invoking static abstract method through reflection at the time of writing (2026/02/15)
		//   See https://github.com/dotnet/runtime/issues/79331
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
}

class FullyResolvedConstrainedValueTypeInfo : ConstrainedTypeInfo
{
	public FullyResolvedConstrainedValueTypeInfo(
		Type type,
		NonEmptyArray<Type> rootTypes,
		MethodInfo instanciationMethod,
		LambdaExpression convertorToNativeRootTypeExpression)
	: base(
		type,
		rootTypes,
		instanciationMethod,
		convertorToNativeRootTypeExpression)
	{
	}
}

class FullyResolvedConstrainedCollectionTypeInfo : ConstrainedTypeInfo
{
	public FullyResolvedConstrainedCollectionTypeInfo(
		Type type,
		NonEmptyArray<Type> rootTypes,
		MethodInfo instanciationMethod,
		LambdaExpression convertorToNativeRootTypeExpression)
	: base(
		type,
		rootTypes,
		instanciationMethod,
		convertorToNativeRootTypeExpression)
	{
	}
}

class FullyResolvedConstrainedCollectionOfKeyValuePairsTypeInfo : ConstrainedTypeInfo
{
	public FullyResolvedConstrainedCollectionOfKeyValuePairsTypeInfo(
		Type type,
		NonEmptyArray<Type> rootTypes,
		MethodInfo instanciationMethod,
		LambdaExpression convertorToNativeRootTypeExpression)
	: base(
		type,
		rootTypes,
		instanciationMethod,
		convertorToNativeRootTypeExpression)
	{
	}
}
