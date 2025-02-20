namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;

enum TypeCategory
{
	TechnicalDefaultEnumValue = 0,
	UnrelatedToConstrainedType = 1,
	ConstrainedValueType = 2,
	ConstrainedGenericCollectionType = 3,
	ConstrainedNonGenericCollectionType = 4,
	ConstrainedGenericCollectionOfKeyValuePairsType = 5,
	ConstrainedNonGenericCollectionOfKeyValuePairsType = 6,
	DataStructureInvolvingAConstrainedType = 7,
	CollectionInvolvingAConstrainedType = 8,
	CollectionOfKeyValuePairsInvolvingAConstrainedType = 9,
	ObjectTypeSpecificallyGeneratedForSerializationPurposes = 10,
	CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes = 11,
	DictionaryInvolvingValuesWhoseTypeWasSpecificallyGeneratedForSerializationPurposes = 12,
}
