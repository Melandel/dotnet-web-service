using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericSortedSetConverter : SimpleCollectionConversionLambdaExpressionBuilder
{
	static SortedSet<T> ToSortedSet<T>(IEnumerable<T> source) => new(source);
	public static readonly CollectionToGenericSortedSetConverter Instance = new();
	static readonly MethodInfo OpenGenericToSortedSetMethod = typeof(CollectionToGenericSortedSetConverter)
		.GetMethod(
			nameof(ToSortedSet),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToSortedSetMethod;
}
