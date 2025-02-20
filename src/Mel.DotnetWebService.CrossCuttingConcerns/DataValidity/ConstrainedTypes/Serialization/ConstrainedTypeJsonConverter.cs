using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.WritingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;

public class ConstrainedTypeJsonConverter : JsonConverter<object>
{
	public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var op = ConstrainedTypeConverterReadingOperation.For(typeToConvert);
		var optWithoutConverter = options.Without<ConstrainedTypeJsonConverter>();

		return op.Execute(ref reader, typeToConvert, options, optWithoutConverter);
	}

	public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		ConstrainedTypeConverterWritingOperation.For(value).Execute(ref writer, value, options);
	}

	public override void WriteAsPropertyName(Utf8JsonWriter writer, object obj, JsonSerializerOptions options)
	=> writer.WritePropertyName(JsonSerializer.Serialize(obj, options).Trim('"'));

	public override bool CanConvert(Type typeToConvert)
	=> IsAbleToConvert(typeToConvert);

    public static bool IsAbleToConvert(Type typeToConvert)
    {
		return TypeCategoryResolver.ResolveFor(typeToConvert) switch
		{
			TypeCategory.ObjectTypeSpecificallyGeneratedForSerializationPurposes => true,
			TypeCategory.CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes => true,
			TypeCategory.DictionaryInvolvingValuesWhoseTypeWasSpecificallyGeneratedForSerializationPurposes => true,
			TypeCategory.ConstrainedGenericCollectionType => true,
			TypeCategory.ConstrainedNonGenericCollectionType => true,
			TypeCategory.ConstrainedValueType => true,
			TypeCategory.DataStructureInvolvingAConstrainedType => true,
			TypeCategory.CollectionInvolvingAConstrainedType => true,
			TypeCategory.CollectionOfKeyValuePairsInvolvingAConstrainedType => true,
			TypeCategory.UnrelatedToConstrainedType => false,
			var typeCategory => throw new NotImplementedException($"{nameof(TypeCategory)} {typeCategory} should never happen, but it has")
};
    }
}
