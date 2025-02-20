using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.WritingOperations;

class KeyValuePairWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly KeyValuePairWritingOperation Instance = new();
	KeyValuePairWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		var type = value.GetType();
		var key = type.GetProperty("Key").GetValue(value);
		writer.WritePropertyName(JsonSerializer.Serialize(key).Trim('"'));

		var valueToSerialize = type.GetProperty("Value").GetValue(value);
		var serializedValue = JsonSerializer.Serialize(valueToSerialize, options);
		writer.WriteRawValue(serializedValue);
	}
}
