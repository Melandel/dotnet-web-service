using System.Collections;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.WritingOperations;

class KeyValuePairsWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly KeyValuePairsWritingOperation Instance = new();
	KeyValuePairsWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		if (value is IDictionary asDictionary)
		{
			foreach (DictionaryEntry dictionaryEntry in asDictionary)
			{
				WriteDictionaryEntry(dictionaryEntry, ref writer, ref options);
			}
		}
		else if (value.GetType().ImplementsGenericIEnumerableOfKeyPairValues(out var kType, out var vType))
		{
			foreach (dynamic keyValuePair in (IEnumerable)value)
			{
				WriteDynamicKeyValuePair(keyValuePair, ref writer, ref options);
			}
		}
		
		writer.WriteEndObject();
	}

	static void WriteDynamicKeyValuePair(dynamic item, ref Utf8JsonWriter writer, ref JsonSerializerOptions options)
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
		var serializedValue = valueToSerialize is null
				? JsonSerializer.Serialize<object>(null!, options)
				: JsonSerializer.Serialize(valueToSerialize, options);
		writer.WriteRawValue(serializedValue);
	}

	static void WriteDictionaryEntry(DictionaryEntry item, ref Utf8JsonWriter writer, ref JsonSerializerOptions options)
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
}
