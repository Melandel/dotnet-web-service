using System.Collections;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.WritingOperations;

class DictionaryWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly DictionaryWritingOperation Instance = new();
	DictionaryWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		foreach (DictionaryEntry item in (IDictionary)value)
		{
			var key = item.Key switch
			{
				ObjectTypeSpecificallyGeneratedForSerializationPurposes k => k.Value,
				var k => k
			};

			var serializedKey = JsonSerializer.Serialize(key, options).Trim('"');
			writer.WritePropertyName(serializedKey);

			var valueToSerialize = item.Value switch
			{
				ObjectTypeSpecificallyGeneratedForSerializationPurposes v => v.Value,
				var v => v
			};
			var serializedValue = JsonSerializer.Serialize(valueToSerialize, options);
			writer.WriteRawValue(serializedValue);
		}
		writer.WriteEndObject();
	}
}
