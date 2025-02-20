using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericStackConverter : SimpleCollectionConversionLambdaExpressionBuilder
{
	static Stack<T> ToStack<T>(IEnumerable<T> source) => new(source);
	public static readonly CollectionToGenericStackConverter Instance = new();
	static readonly MethodInfo OpenGenericToStackMethod = typeof(CollectionToGenericStackConverter)
		.GetMethod(
			nameof(ToStack),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToStackMethod;
}
