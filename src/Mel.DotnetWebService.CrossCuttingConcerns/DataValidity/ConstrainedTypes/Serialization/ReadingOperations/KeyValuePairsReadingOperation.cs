using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

class KeyValuePairsReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly KeyValuePairsReadingOperation Instance = new();
	KeyValuePairsReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartObject} json token, but is being called on {reader.TokenType} json token instead");
		}

		var keyValuePairCollectionBuilder = KeyValuePairCollectionBuilder.For(targetType);
		var keyType = keyValuePairCollectionBuilder.KeyType;
		var valueType = keyValuePairCollectionBuilder.ValueType;

		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				break;
			}

			var propertyNameReader = reader.GetUtf8JsonReaderForPropertyName(keyType);
			dynamic key = For(keyType).Execute(ref propertyNameReader, keyType, options, preComputedOptionsWithoutConstrainedTypeConverter)!;

			reader.Read();
			dynamic value = For(valueType).Execute(ref reader, valueType, options, preComputedOptionsWithoutConstrainedTypeConverter)!;

			keyValuePairCollectionBuilder.Add(key, value);
		}

		if (reader.TokenType != JsonTokenType.EndObject)
		{
			throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must complete on {JsonTokenType.EndObject} json token, but is being completed on {reader.TokenType} json token instead");
		}

		var keyValuePairCollection = keyValuePairCollectionBuilder.Build();
		return keyValuePairCollection;
	}
}
