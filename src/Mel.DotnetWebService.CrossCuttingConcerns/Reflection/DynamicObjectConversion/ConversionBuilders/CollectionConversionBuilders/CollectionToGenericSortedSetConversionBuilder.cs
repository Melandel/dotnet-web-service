using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

class CollectionToGenericSortedSetConversionBuilder : SimpleCollectionConversionBuilder
{
	static SortedSet<T> ToSortedSet<T>(IEnumerable<T> source) => new(source);
	public static readonly CollectionToGenericSortedSetConversionBuilder Instance = new();
	static readonly MethodInfo OpenGenericToSortedSetMethod = typeof(CollectionToGenericSortedSetConversionBuilder)
		.GetMethod(
			nameof(ToSortedSet),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToSortedSetMethod;
}
