using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class DictionaryReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly DictionaryReadingOperation Instance = new();
	DictionaryReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options)
	{
		return null;
	}
}
