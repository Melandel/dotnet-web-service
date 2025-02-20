using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericLinkedListConverter : SimpleCollectionConversionLambdaExpressionBuilder
{
	static LinkedList<T> ToLinkedList<T>(IEnumerable<T> source) => new LinkedList<T>(source);
	public static readonly CollectionToGenericLinkedListConverter Instance = new();
	static readonly MethodInfo OpenGenericToLinkedListMethod = typeof(CollectionToGenericLinkedListConverter)
		.GetMethod(
			nameof(ToLinkedList),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToLinkedListMethod;
}
