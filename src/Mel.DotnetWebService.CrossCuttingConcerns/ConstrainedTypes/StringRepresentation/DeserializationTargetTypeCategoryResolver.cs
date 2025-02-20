using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.WritingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

static class DeserializationTargetTypeCategoryResolver
{
	public static DeserializationTargetTypeCategory ResolveFor(Type targetType)
	=> targetType switch
	{
		var type when IsAConstainedType(type) => DeserializationTargetTypeCategory.ConstrainedType,
		var type when IsADataStructureThatConstainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(type) => DeserializationTargetTypeCategory.DataStructureThatContainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems,
		var type when IsRecursiveCollectionOfConstrainedTypeItems(type) => DeserializationTargetTypeCategory.Collection,
		var type when IsRecursiveCollectionOfItemsWhoseTypeContainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(type) => DeserializationTargetTypeCategory.Collection,
		_ => DeserializationTargetTypeCategory.ThatDoesNotInvolveAnyConstrainedType
	};

	static bool IsAConstainedType(Type t)
	{
		return t.IsAConstrainedType(out _);
	}

	static bool IsADataStructureThatConstainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(Type t)
	{
		return !t.IsEnumerableWithoutBeingString() && t.HasFieldOrPropertyWithConstrainedType(browseRecursively: true);
	}

	static bool IsDictionaryGeneratedForSerializationPurposes(Type t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) && t.GetGenericArguments().First() == typeof(string) && t.GetGenericArguments().Last() == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes);
	}
	static bool IsRecursiveCollectionOfConstrainedTypeItems(Type t)
	=> t.IsEnumerableWithoutBeingString() && t.ImplementsGenericIEnumerableWithAnArgumentTypeThatVerifies(arg => arg.IsAConstrainedType() || IsRecursiveCollectionOfConstrainedTypeItems(arg));

	static bool IsRecursiveCollectionOfItemsWhoseTypeContainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(Type t)
	=> t.IsEnumerableWithoutBeingString() && t.ImplementsGenericIEnumerableWithAnArgumentTypeThatVerifies(arg => arg.IsOrInvolvesAConstrainedType() || IsRecursiveCollectionOfItemsWhoseTypeContainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(arg));

	static bool IsConstrainedCollectionType(Type type)
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>), genericArgType => genericArgType.ImplementsInterface(typeof(IEnumerable<>)));

	static bool IsAConstrainedType(Type type)
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>));
}
