using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.WritingOperations;

class CollectionWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly CollectionWritingOperation Instance = new();
	CollectionWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();

		foreach (var item in (System.Collections.IEnumerable)value)
		{
			var serialized = JsonSerializer.Serialize(item, options);
			writer.WriteRawValue(serialized);
		}

		writer.WriteEndArray();
	}
}
