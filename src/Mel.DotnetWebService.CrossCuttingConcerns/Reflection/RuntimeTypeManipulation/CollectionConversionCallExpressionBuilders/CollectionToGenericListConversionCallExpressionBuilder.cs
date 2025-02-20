using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericListConverter : SimpleCollectionConversionLambdaExpressionBuilder
{
	public static readonly CollectionToGenericListConverter Instance = new();
	static readonly MethodInfo OpenGenericToListMethod = EnumerableGenericMethods
		.Single(m => m.Name == nameof(Enumerable.ToList));

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToListMethod;
}
