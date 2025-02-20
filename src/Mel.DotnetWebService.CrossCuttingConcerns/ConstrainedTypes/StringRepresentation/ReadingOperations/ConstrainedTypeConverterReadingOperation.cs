using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

abstract class ConstrainedTypeConverterReadingOperation
{
	public abstract object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options);

	public static ConstrainedTypeConverterReadingOperation For(Type targetType)
	{
		var v = DeserializationTargetTypeCategoryResolver.ResolveFor(targetType);
		return v switch
		{
			DeserializationTargetTypeCategory.ConstrainedType => ConstrainedDataTypeReadingOperation.Instance,
			DeserializationTargetTypeCategory.DataStructureThatContainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems => TypeInvolvingConstrainedTypeReadingOperation.Instance,
			DeserializationTargetTypeCategory.Collection => CollectionReadingOperation.Instance,
			DeserializationTargetTypeCategory.ThatDoesNotInvolveAnyConstrainedType => DefaultReadingOperation.Instance,
			_ => throw new NotImplementedException()
		};
	}
}
