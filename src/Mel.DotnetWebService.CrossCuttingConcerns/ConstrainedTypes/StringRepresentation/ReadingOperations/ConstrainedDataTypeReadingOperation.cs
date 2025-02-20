using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class ConstrainedDataTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly ConstrainedDataTypeReadingOperation Instance = new();
	ConstrainedDataTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var typeExpectedInsideJson = targetType switch
		{
			var t when t.ImplementsGenericInterface(typeof(ICanBeDeserializedFromJson<,>), out var argumentTypes) => argumentTypes.First(),
			var t => t.GetConstrainedTypeRootType()
		};

		var valueFoundInsideJson = typeExpectedInsideJson switch
		{
			_ when reader.NativelySupports(typeExpectedInsideJson) => ConstrainedTypesIgnoringReadingOperation.Instance.Execute(ref reader, typeExpectedInsideJson, options, preComputedOptionsWithoutConstrainedTypeConverter),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {typeof(Constrained<>).Name} type is being deserializd from {reader.TokenType} json token, but is declared as {typeof(ICanBeDeserializedFromJson<,>).GetGenericTypeDefinition().Name}<{typeExpectedInsideJson.Name},_>, which is not coherent.")
		};

		return targetType.ReconstituteFromValueFoundInsideJson(valueFoundInsideJson);
	}
}
