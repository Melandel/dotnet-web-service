using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.WritingOperations;

abstract class ConstrainedTypeConverterWritingOperation
{
	public abstract void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options);

	public static ConstrainedTypeConverterWritingOperation For(object value)
	=> SerializationSourceTypeCategoryResolver.ResolveFor(value) switch
	{
		SerializationSourceTypeCategory.ObjectTypeSpecificallyGeneratedForSerializationPurposes => ObjectTypeSpecificallyGeneratedForSerializationPurposesWritingOperation.Instance,
		SerializationSourceTypeCategory.CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes => CollectionWritingOperation.Instance,
		SerializationSourceTypeCategory.Dictionary => DictionaryWritingOperation.Instance,
		SerializationSourceTypeCategory.CollectionOfContrainedTypeItems => CollectionWritingOperation.Instance,
		SerializationSourceTypeCategory.ConstrainedCollectionType => DataMatchingConstructorParametersAndPublicPropertiesWritingOperation.Instance,
		SerializationSourceTypeCategory.ConstrainedType => ConstrainedDataTypeWritingOperation.Instance,
		SerializationSourceTypeCategory.KeyValuePair => KeyValuePairWritingOperation.Instance,
		SerializationSourceTypeCategory.ThatDoesNotInvolveAnyConstrainedType => DataMatchingConstructorParametersAndPublicPropertiesWritingOperation.Instance,
		_ => throw new NotImplementedException()
	};
}
