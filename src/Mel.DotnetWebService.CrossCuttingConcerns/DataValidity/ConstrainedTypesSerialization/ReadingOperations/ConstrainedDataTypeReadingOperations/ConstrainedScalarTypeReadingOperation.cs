using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.ConstrainedDataTypeReadingOperations;

class ConstrainedScalarTypeReadingOperation : ConstrainedTypeReadingOperation
{
	public static readonly ConstrainedScalarTypeReadingOperation Instance = new();
	ConstrainedScalarTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var typeExpectedInsideJson = targetType switch
		{
			var t when t.ImplementsGenericInterface(typeof(ICanBeDeserializedFromJson<,>), out var argumentTypes) => argumentTypes.First() switch
			{
				var expectedType when expectedType.ImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) => argTypes.First().MakeArrayType(),
				var expectedType => expectedType
			},
			var t => t.GetConstrainedTypeRootType() switch
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
			_ => throw new InvalidOperationException($"{targetType.GetName()} : {typeof(Constrained<>).Name[..^2]}<{targetType.GetConstrainedTypeRootType().GetName()}> type is being deserialized from {reader.TokenType} json token, but is declared as {typeof(ICanBeDeserializedFromJson<,>).GetGenericTypeDefinition().Name[..^2]}<{typeExpectedInsideJson.GetName()},_>, which is not coherent.")
		};

		return targetType.ReconstituteFromValueFoundInsideJson(valueFoundInsideJson);
	}
}
