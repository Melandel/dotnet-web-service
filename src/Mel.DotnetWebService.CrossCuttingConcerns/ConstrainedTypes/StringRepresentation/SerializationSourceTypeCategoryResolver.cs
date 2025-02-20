using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.WritingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

static class SerializationSourceTypeCategoryResolver
{
	public static SerializationSourceTypeCategory ResolveFor(object value)
	=> value.GetType() switch
	{
		var type when IsObjectTypeGeneratedForSerializationPurposes(type) => SerializationSourceTypeCategory.ObjectTypeSpecificallyGeneratedForSerializationPurposes,
		var type when IsCollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes(type) => SerializationSourceTypeCategory.CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes,
		var type when IsDictionary(type) => SerializationSourceTypeCategory.Dictionary,
		var type when IsCollectionOfContrainedTypeItems(type) => SerializationSourceTypeCategory.CollectionOfContrainedTypeItems,
		var type when IsConstrainedCollectionType(type) => SerializationSourceTypeCategory.ConstrainedCollectionType,
		var type when IsAConstrainedType(type) => SerializationSourceTypeCategory.ConstrainedType,
		var type when type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>) => SerializationSourceTypeCategory.KeyValuePair,
		_ => SerializationSourceTypeCategory.ThatDoesNotInvolveAnyConstrainedType
	};

	static bool IsObjectTypeGeneratedForSerializationPurposes(Type t)
	{
		return t == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes);
	}

	static bool IsCollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes(Type t)
	{
		return t.IsReadOnlyCollectionWithoutBeingString() && (t.IsAGenericCollectionType(argType => argType == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes)) || t.IsAGenericDictionaryType(argType => argType == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes)));
	}

	static bool IsDictionaryGeneratedForSerializationPurposes(Type t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) && t.GetGenericArguments().First() == typeof(string) && t.GetGenericArguments().Last() == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes);
	}
	static bool IsDictionary(Type type)
	=> type.GetInterfaces().Any(itf => itf == typeof(System.Collections.IDictionary));

	static bool IsCollectionOfContrainedTypeItems(Type type)
	=> type.GetInterfaces().Any(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IEnumerable<>));

	static bool IsConstrainedCollectionType(Type type)
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>), genericArgType => genericArgType.ImplementsInterface(typeof(IEnumerable<>)));

	static bool IsAConstrainedType(Type type)
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>));
}
