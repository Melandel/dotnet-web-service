using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.WritingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;

static class TypeCategoryResolver
{
	static readonly Dictionary<Type, TypeCategory> categoriesByType = new Dictionary<Type, TypeCategory>();
	public static TypeCategory ResolveFor(Type type)
	{
		if (!categoriesByType.ContainsKey(type))
		{
			categoriesByType.Add(
				type,
				type switch
				{
					var t when IsDictionary<string, ObjectTypeSpecificallyGeneratedForSerializationPurposes>(t) => TypeCategory.DictionaryInvolvingValuesWhoseTypeWasSpecificallyGeneratedForSerializationPurposes,
					var t when IsCollectionOf<ObjectTypeSpecificallyGeneratedForSerializationPurposes>(t) => TypeCategory.CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes,
					var t when Is<ObjectTypeSpecificallyGeneratedForSerializationPurposes>(t) => TypeCategory.ObjectTypeSpecificallyGeneratedForSerializationPurposes,
					var t when IsDictionaryInvolvingZeroOrNLevelsOfCollectionsOrDictionariesWhoseItemsContainAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(t) => TypeCategory.DictionaryInvolvingAConstrainedType,
					var t when IsCollectionOfZeroOrNLevelsOfCollectionsOrDictionariesWhoseItemsContainAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(t) => TypeCategory.CollectionInvolvingAConstrainedType,
					var t when IsAConstainedType(t) => TypeCategory.ConstrainedType,
					var t when IsADataStructureThatConstainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(t) => TypeCategory.DataStructureInvolvingAConstrainedType,
					_ => TypeCategory.UnrelatedToConstrainedType
				});
		}

		return categoriesByType[type];
	}
	static bool Is<T>(Type t)
	{
		return t == typeof(T);
	}

	static bool IsCollectionOf<T>(Type t)
	{
		return t.IsReadOnlyCollectionWithoutBeingString() && (t.IsAGenericCollectionType(argType => argType == typeof(T)) || t.IsAGenericDictionaryType(argType => argType == typeof(T)));
	}
	static bool IsKeyValuePair<TKey, TValue>(Type t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(KeyValuePair<,>) && t.GetGenericArguments().First() == typeof(TKey) && t.GetGenericArguments().Last() == typeof(TValue);
	}

	static bool IsDictionary<TKey, TValue>(Type t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) && t.GetGenericArguments().First() == typeof(TKey) && t.GetGenericArguments().Last() == typeof(TValue);
	}
	static bool IsAConstainedType(Type t)
	{
		return t.IsAConstrainedType(out _);
	}

	static bool IsADataStructureThatConstainsAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(Type t)
	{
		return t.IsOrInvolvesAConstrainedType();
	}

	static bool IsCollectionOfZeroOrNLevelsOfCollectionsOrDictionariesWhoseItemsContainAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(Type t)
	=> t.IsEnumerableWithoutBeingString()
		&& t.ImplementsGenericIEnumerableWithAnArgumentTypeThatVerifies(argType =>
			argType.IsOrInvolvesAConstrainedType()
		|| IsCollectionOfZeroOrNLevelsOfCollectionsOrDictionariesWhoseItemsContainAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(argType)
		|| IsDictionaryInvolvingZeroOrNLevelsOfCollectionsOrDictionariesWhoseItemsContainAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(argType));

	static bool IsDictionaryInvolvingZeroOrNLevelsOfCollectionsOrDictionariesWhoseItemsContainAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(Type t)
	=> t.IsAGenericDictionaryType(argType =>
		argType.IsOrInvolvesAConstrainedType()
		|| IsCollectionOfZeroOrNLevelsOfCollectionsOrDictionariesWhoseItemsContainAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(argType)
		|| IsDictionaryInvolvingZeroOrNLevelsOfCollectionsOrDictionariesWhoseItemsContainAConstrainedTypeEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems(argType));
}
