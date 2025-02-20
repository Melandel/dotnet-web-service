using System.Reflection;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataTypeReadingOperations;

class ConstrainedScalarTypeReadingOperation : ConstrainedTypeReadingOperation
{
	const string ConstrainedTypeStaticFactoryMethodName = nameof(IConstrainedValue<int, ConstrainedInt>.ApplyConstraintsTo);
	static readonly Dictionary<Type, MethodInfo> EncounteredReconstitutionMethodsByType = new Dictionary<Type, MethodInfo>();
	public static readonly ConstrainedScalarTypeReadingOperation Instance = new();
	ConstrainedScalarTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var typeExpectedInsideJson = targetType switch
		{
			var t when t.ImplementsGenericInterface(typeof(IConstrainedValue<,>), out var argumentTypes) => argumentTypes.First() switch
			{
				var expectedType when expectedType.ImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) => argTypes.First().MakeArrayType(),
				var expectedType => expectedType
			},
			var t => ConstrainedTypeInfos.GetRootType(t) switch
			{
				var expectedType when expectedType.ImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) => argTypes.First().MakeArrayType(),
				var expectedType => expectedType
			}
		};

		if (targetType.IsAFirstClassCollection(out var constructorParameterType, out var instanciationOperationParameterTypeCandidates))
		{
			return ConstrainedCollectionTypeReadingOperation
				.Instance(targetType, constructorParameterType, instanciationOperationParameterTypeCandidates)
				.Execute(ref reader, targetType, options, preComputedOptionsWithoutConstrainedTypeConverter);
		}

		var valueFoundInsideJson = typeExpectedInsideJson switch
		{
			_ when reader.NativelySupports(typeExpectedInsideJson) => ConstrainedTypesIgnoringReadingOperation.Instance.Execute(ref reader, typeExpectedInsideJson, options, preComputedOptionsWithoutConstrainedTypeConverter),
			_ => throw new InvalidOperationException($"{targetType.GetName()} : {typeof(ConstrainedValue<>).Name[..^2]}<{ConstrainedTypeInfos.GetRootType(targetType).GetName()}> type is being deserialized from {reader.TokenType} json token, but is declared as {typeof(IConstrainedValue<,>).GetGenericTypeDefinition().Name[..^2]}<{typeExpectedInsideJson.GetName()},_>, which is not coherent.")
		};
		var reconstituted = ConstrainedTypeInfos.ReconstituteFromRootTypeValue(targetType, valueFoundInsideJson!);
		return reconstituted;
	}
}
