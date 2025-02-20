using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericQueueConverter : SimpleCollectionConversionLambdaExpressionBuilder
{
	static Queue<T> ToQueue<T>(IEnumerable<T> source) => new(source);
	public static readonly CollectionToGenericQueueConverter Instance = new();
	static readonly MethodInfo OpenGenericToQueueMethod = typeof(CollectionToGenericQueueConverter)
		.GetMethod(
			nameof(ToQueue),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToQueueMethod;
}
