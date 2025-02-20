namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

public enum CollectionOfKeyValuePairsCategory
{
	TechnicalDefaultEnumValue = 0,
	IsRelatedToGenericKeyValuePairIEnumerables = 1, // composition, implementation
	IsNotRelatedToGenericKeyValuePairIEnumerables = 2 // non-generics: Array, IDictionary, ICollection
}
