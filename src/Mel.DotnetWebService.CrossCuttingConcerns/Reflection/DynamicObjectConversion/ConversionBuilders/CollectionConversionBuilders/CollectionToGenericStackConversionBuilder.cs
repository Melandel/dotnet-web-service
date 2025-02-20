using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

class CollectionToGenericStackConversionBuilder : SimpleCollectionConversionBuilder
{
	static Stack<T> ToStack<T>(IEnumerable<T> source) => new(source);
	public static readonly CollectionToGenericStackConversionBuilder Instance = new();
	static readonly MethodInfo OpenGenericToStackMethod = typeof(CollectionToGenericStackConversionBuilder)
		.GetMethod(
			nameof(ToStack),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToStackMethod;
}
