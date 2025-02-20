using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

class CollectionToGenericQueueConversionBuilder : SimpleCollectionConversionBuilder
{
	static Queue<T> ToQueue<T>(IEnumerable<T> source) => new(source);
	public static readonly CollectionToGenericQueueConversionBuilder Instance = new();
	static readonly MethodInfo OpenGenericToQueueMethod = typeof(CollectionToGenericQueueConversionBuilder)
		.GetMethod(
			nameof(ToQueue),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToQueueMethod;
}
