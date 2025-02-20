using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class KeyValuePairReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly KeyValuePairReadingOperation Instance = new();
	KeyValuePairReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options)
	{
		return null;
	}
}
