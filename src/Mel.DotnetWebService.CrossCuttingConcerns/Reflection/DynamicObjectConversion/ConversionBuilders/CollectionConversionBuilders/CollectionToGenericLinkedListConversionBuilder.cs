using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

class CollectionToGenericLinkedListConversionBuilder : SimpleCollectionConversionBuilder
{
	static LinkedList<T> ToLinkedList<T>(IEnumerable<T> source) => new LinkedList<T>(source);
	public static readonly CollectionToGenericLinkedListConversionBuilder Instance = new();
	static readonly MethodInfo OpenGenericToLinkedListMethod = typeof(CollectionToGenericLinkedListConversionBuilder)
		.GetMethod(
			nameof(ToLinkedList),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToLinkedListMethod;
}
