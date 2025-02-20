using System.Collections.Concurrent;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.WritingOperations;

abstract class ConstrainedTypeConverterWritingOperation
{
	public abstract void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options);

	static readonly ConcurrentDictionary<Type, ConstrainedTypeConverterWritingOperation> WritingOperationsByType = new ConcurrentDictionary<Type, ConstrainedTypeConverterWritingOperation>();
	public static ConstrainedTypeConverterWritingOperation For(object value)
	=> WritingOperationsByType.GetOrAdd(
		value.GetType(),
		_ => TypeCategoryResolver.ResolveFor(value.GetType()) switch
		{
			TypeCategory.DictionaryInvolvingValuesWhoseTypeWasSpecificallyGeneratedForSerializationPurposes => KeyValuePairsWritingOperation.Instance,
			TypeCategory.CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes => CollectionWritingOperation.Instance,
			TypeCategory.ObjectTypeSpecificallyGeneratedForSerializationPurposes => ObjectTypeSpecificallyGeneratedForSerializationPurposesWritingOperation.Instance,
			TypeCategory.ConstrainedGenericCollectionType => CollectionWritingOperation.Instance,
			TypeCategory.ConstrainedNonGenericCollectionType => ConstrainedDataTypeWritingOperation.Instance,
			TypeCategory.ConstrainedGenericCollectionOfKeyValuePairsType => KeyValuePairsWritingOperation.Instance,
			TypeCategory.ConstrainedNonGenericCollectionOfKeyValuePairsType => KeyValuePairsWritingOperation.Instance,
			TypeCategory.CollectionOfKeyValuePairsInvolvingAConstrainedType => KeyValuePairsWritingOperation.Instance,
			TypeCategory.CollectionInvolvingAConstrainedType => CollectionWritingOperation.Instance,
			TypeCategory.DataStructureInvolvingAConstrainedType => PublicPropertiesWritingOperation.Instance,
			TypeCategory.ConstrainedValueType => ConstrainedDataTypeWritingOperation.Instance,
			TypeCategory.UnrelatedToConstrainedType => ConstrainedTypesIgnoringWritingOperation.Instance,
			TypeCategory.TechnicalDefaultEnumValue => throw new NotImplementedException($"{nameof(TypeCategory)}.{nameof(TypeCategory.TechnicalDefaultEnumValue)}"),
			_ => throw new NotImplementedException()
		});
}
