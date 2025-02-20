using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericArrayConverter : SimpleCollectionConversionLambdaExpressionBuilder
{
	public static readonly CollectionToGenericArrayConverter Instance = new();
	static readonly MethodInfo OpenGenericToArrayMethod = EnumerableGenericMethods
		.Single(m => m.Name == nameof(Enumerable.ToArray));

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToArrayMethod;
}

