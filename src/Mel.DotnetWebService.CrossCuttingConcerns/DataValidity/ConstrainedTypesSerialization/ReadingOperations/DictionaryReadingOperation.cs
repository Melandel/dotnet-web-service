using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations;

class DictionaryReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly DictionaryReadingOperation Instance = new();
	DictionaryReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		_ = reader.TokenType switch
		{
			JsonTokenType.StartObject => reader.Read(),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartObject} json token, but is being called on {reader.TokenType} json token instead")
		};

		var dictionaryGenericArgs = targetType.GetGenericArguments();
		var keyType = dictionaryGenericArgs.First();
		var valueType = dictionaryGenericArgs.Last();
		var dictionary = Activator.CreateInstance(targetType);

		while (reader.TokenType != JsonTokenType.EndObject)
		{
			var propertyNameReader = reader.GetUtf8JsonReaderForPropertyName();
			reader.Read();
			var key = For(keyType).Execute(ref propertyNameReader, keyType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			var value = For(valueType).Execute(ref reader, valueType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			targetType.InvokeMember(
				nameof(IDictionary.Add),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				dictionary,
				new object?[] { key, value });
		}

		_ = reader.TokenType switch
		{
			JsonTokenType.EndObject => reader.Read(),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must complete on {JsonTokenType.EndObject} json token, but is being completed on {reader.TokenType} json token instead")
		};

		return dictionary;
	}
}
