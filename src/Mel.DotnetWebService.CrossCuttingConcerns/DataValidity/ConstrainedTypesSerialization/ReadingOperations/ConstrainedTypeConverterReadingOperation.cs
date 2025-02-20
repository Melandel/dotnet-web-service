using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations;

abstract class ConstrainedTypeConverterReadingOperation
{
	public abstract object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter);

	static readonly Dictionary<Type, ConstrainedTypeConverterReadingOperation> readingOperationsByType = new Dictionary<Type, ConstrainedTypeConverterReadingOperation>();
	public static ConstrainedTypeConverterReadingOperation For(Type type)
	{
		if (!readingOperationsByType.ContainsKey(type))
		{
			readingOperationsByType.Add(
				type,
				TypeCategoryResolver.ResolveFor(type) switch
				{
					TypeCategory.ConstrainedType => ConstrainedTypeReadingOperation.InstanceSuitedFor(type),
					TypeCategory.DictionaryInvolvingAConstrainedType => DictionaryReadingOperation.Instance,
					TypeCategory.CollectionInvolvingAConstrainedType => CollectionReadingOperation.Instance,
					TypeCategory.DataStructureInvolvingAConstrainedType => TypeInvolvingConstrainedTypeReadingOperation.InstanceSuitedFor(type),
					_ => ConstrainedTypesIgnoringReadingOperation.Instance
				});
		}

		return readingOperationsByType[type];
	}
}
