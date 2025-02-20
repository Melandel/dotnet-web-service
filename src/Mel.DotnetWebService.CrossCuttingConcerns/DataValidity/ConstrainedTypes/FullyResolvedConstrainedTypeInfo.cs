using System.Collections;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedTypeInfo
{
	public Type Type { get; }
	public NonEmptyArray<Type> RootTypes { get; }
	public Type RootType => RootTypes.Last();
	//public Type NativeRootType => RootTypes.Last();
	//RootType
	readonly Lazy<CompiledInvokable> _instanciator;
	readonly Lazy<CompiledInvokable> _convertorToNativeRootType;
	readonly Lazy<object> _typeExampleValues;

	public dynamic InvokeStaticFactoryMethod(dynamic valueAsNativeRootType)
	=> _instanciator.Value.Invoke(valueAsNativeRootType);

	public dynamic InvokeImplicitConversionToRootType(dynamic valueAsFullyResolvedConstrainedType)
	=> _convertorToNativeRootType.Value.Invoke(valueAsFullyResolvedConstrainedType);

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
		Lazy<CompiledInvokable> instanciator,
		Lazy<CompiledInvokable> convertorToNativeRootType)
	{
		Type = type;
		RootTypes = rootTypes;
		_instanciator = instanciator;
		_convertorToNativeRootType = convertorToNativeRootType;
		_typeExampleValues = new Lazy<object>(
			() => Type
				.GetProperty(
					nameof(IConstrainedValue<int, ConstrainedInt>.Examples),
					BindingFlags.Static | BindingFlags.Public)!
				.GetValue(null, null)!);
	}
}

class FullyResolvedConstrainedValueTypeInfo : ConstrainedTypeInfo
{
	public FullyResolvedConstrainedValueTypeInfo(
		Type type,
		NonEmptyArray<Type> rootTypes,
		Lazy<CompiledInvokable> instanciator,
		Lazy<CompiledInvokable> convertorToNativeRootType)
	: base(
		type,
		rootTypes,
		instanciator,
		convertorToNativeRootType)
	{
	}
}

class FullyResolvedConstrainedCollectionTypeInfo : ConstrainedTypeInfo
{
	public FullyResolvedConstrainedCollectionTypeInfo(
		Type type,
		NonEmptyArray<Type> rootTypes,
		Lazy<CompiledInvokable> instanciator,
		Lazy<CompiledInvokable> convertorToNativeRootType)
	: base(
		type,
		rootTypes,
		instanciator,
		convertorToNativeRootType)
	{
	}
}

class FullyResolvedConstrainedCollectionOfKeyValuePairsTypeInfo : ConstrainedTypeInfo
{
	public FullyResolvedConstrainedCollectionOfKeyValuePairsTypeInfo(
		Type type,
		NonEmptyArray<Type> rootTypes,
		Lazy<CompiledInvokable> instanciator,
		Lazy<CompiledInvokable> convertorToNativeRootType)
	: base(
		type,
		rootTypes,
		instanciator,
		convertorToNativeRootType)
	{
	}
}
