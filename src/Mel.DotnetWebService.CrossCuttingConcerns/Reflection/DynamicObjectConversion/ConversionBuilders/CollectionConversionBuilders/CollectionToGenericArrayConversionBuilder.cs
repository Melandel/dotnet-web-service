using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

class CollectionToGenericArrayConversionBuilder : SimpleCollectionConversionBuilder
{
	public static readonly CollectionToGenericArrayConversionBuilder Instance = new();
	static readonly MethodInfo OpenGenericToArrayMethod = EnumerableGenericMethods
		.Single(m => m.Name == nameof(Enumerable.ToArray));

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToArrayMethod;
}

