namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;

enum TypeCategory
{
	TechnicalDefaultEnumValue = 0,
	UnrelatedToConstrainedType = 1,
	ConstrainedValueType = 2,
	ConstrainedGenericCollectionType = 3,
	ConstrainedNonGenericCollectionType = 4,
	DataStructureInvolvingAConstrainedType = 5,
	CollectionInvolvingAConstrainedType = 6,
	CollectionOfKeyValuePairsInvolvingAConstrainedType = 7,
	ObjectTypeSpecificallyGeneratedForSerializationPurposes = 8,
	CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes = 9,
	DictionaryInvolvingValuesWhoseTypeWasSpecificallyGeneratedForSerializationPurposes = 10,
}
