using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations;

class ConstrainedTypesIgnoringReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly ConstrainedTypesIgnoringReadingOperation Instance = new();
	ConstrainedTypesIgnoringReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var deserialized = JsonSerializer.Deserialize(ref reader, targetType, preComputedOptionsWithoutConstrainedTypeConverter);
		reader.Read();
		return deserialized;
	}
}
