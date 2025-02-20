using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.WritingOperations;

abstract class ConstrainedTypeConverterWritingOperation
{
	public abstract void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options);

	static readonly Dictionary<Type, ConstrainedTypeConverterWritingOperation> writingOperationsByType = new Dictionary<Type, ConstrainedTypeConverterWritingOperation>();
	public static ConstrainedTypeConverterWritingOperation For(object value)
	{
		var type = value.GetType();
		if (!writingOperationsByType.ContainsKey(type))
		{
			writingOperationsByType.Add(
				type,
				TypeCategoryResolver.ResolveFor(type) switch
				{
					TypeCategory.DictionaryInvolvingValuesWhoseTypeWasSpecificallyGeneratedForSerializationPurposes => DictionaryWritingOperation.Instance,
					TypeCategory.CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes => CollectionWritingOperation.Instance,
					TypeCategory.ObjectTypeSpecificallyGeneratedForSerializationPurposes => ObjectTypeSpecificallyGeneratedForSerializationPurposesWritingOperation.Instance,
					TypeCategory.DictionaryInvolvingAConstrainedType => DictionaryWritingOperation.Instance,
					TypeCategory.CollectionInvolvingAConstrainedType => CollectionWritingOperation.Instance,
					TypeCategory.DataStructureInvolvingAConstrainedType => DataMatchingConstructorParametersAndPublicPropertiesWritingOperation.Instance,
					TypeCategory.ConstrainedType => ConstrainedDataTypeWritingOperation.Instance,
					TypeCategory.UnrelatedToConstrainedType => DataMatchingConstructorParametersAndPublicPropertiesWritingOperation.Instance,
					TypeCategory.TechnicalDefaultEnumValue => throw new NotImplementedException($"{nameof(TypeCategory)}.{nameof(TypeCategory.TechnicalDefaultEnumValue)}"),
					_ => throw new NotImplementedException()
				});
		}

		return writingOperationsByType[type];
	}
}
