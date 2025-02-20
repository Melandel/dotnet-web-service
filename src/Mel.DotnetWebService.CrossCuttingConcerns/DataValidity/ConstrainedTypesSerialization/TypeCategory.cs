namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;

enum TypeCategory
{
	TechnicalDefaultEnumValue = 0,
	UnrelatedToConstrainedType = 1,
	ConstrainedType = 2,
	DataStructureInvolvingAConstrainedType = 3,
	CollectionInvolvingAConstrainedType = 4,
	DictionaryInvolvingAConstrainedType = 5,
	ObjectTypeSpecificallyGeneratedForSerializationPurposes = 6,
	CollectionOfItemsWhoseTypeWasSpecificallyGeneratedForSerializationPurposes = 7,
	DictionaryInvolvingValuesWhoseTypeWasSpecificallyGeneratedForSerializationPurposes = 8,
}
