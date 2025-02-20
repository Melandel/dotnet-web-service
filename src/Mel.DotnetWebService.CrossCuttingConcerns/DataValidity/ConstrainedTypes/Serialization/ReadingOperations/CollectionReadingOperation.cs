using System.Collections;
using System.Reflection;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypedListConverters;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

class CollectionReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly CollectionReadingOperation Instance = new();
	CollectionReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		if (reader.TokenType != JsonTokenType.StartArray)
		{
			throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartArray} json token, but is being called on {reader.TokenType} json token instead");
		}

		var itemType = targetType.GetCollectionItemType();
		var genericListBuilder = GenericListBuilder.For(itemType);

		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndArray)
			{
				break;
			}

			var deserialized =
				For(itemType)
				.Execute(ref reader, itemType, options, preComputedOptionsWithoutConstrainedTypeConverter)!;
			genericListBuilder.Add(deserialized);
		}

		if (reader.TokenType != JsonTokenType.EndArray)
		{
			throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must complete on {JsonTokenType.EndArray} json token, but is being completed on {reader.TokenType} json token instead");
		}

		var collectionWithExpectedType = TypedListConverter
			.InstanceSuitedFor(targetType)
			.Convert(genericListBuilder.BuildAsIList(), itemType, targetType);

		return collectionWithExpectedType;
	}
}
