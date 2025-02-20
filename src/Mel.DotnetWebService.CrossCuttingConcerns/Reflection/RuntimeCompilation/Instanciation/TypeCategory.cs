namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public enum TypeCategory
{
	TechnicalDefaultEnumValue = 0,
	NativeValueType = 1,
	EnumType = 2,
	CollectionType = 3,
	UnconstrainedCollectionOfKeyValuePairsType = 4,
	ParentObjectType = 5
}
