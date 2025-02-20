using System.Collections;
using System.Reflection;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypedListConverters;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations;

class CollectionReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly CollectionReadingOperation Instance = new();
	CollectionReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		_ = reader.TokenType switch
		{
			JsonTokenType.StartArray => reader.Read(),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartArray} json token, but is being called on {reader.TokenType} json token instead")
		};

		var itemType = targetType.GetCollectionItemType();
		var listType = typeof(List<>).MakeGenericType(itemType);
		dynamic list = Activator.CreateInstance(listType)!;

		while (reader.TokenType != JsonTokenType.EndArray)
		{
			var deserialized =
				For(itemType)
				.Execute(ref reader, itemType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			listType.InvokeMember(
				nameof(IList.Add),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				list,
				new object?[] { deserialized });
		}

		_ = reader.TokenType switch
		{
			JsonTokenType.EndArray => reader.Read(),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must complete on {JsonTokenType.EndArray} json token, but is being completed on {reader.TokenType} json token instead")
		};

		var collectionWithExpectedType = TypedListConverter
			.InstanceSuitedFor(targetType)
			.Convert(list, itemType, targetType);

		return collectionWithExpectedType;
	}
}
