using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

class CollectionToGenericListConversionBuilder : SimpleCollectionConversionBuilder
{
	public static readonly CollectionToGenericListConversionBuilder Instance = new();
	static readonly MethodInfo OpenGenericToListMethod = EnumerableGenericMethods
		.Single(m => m.Name == nameof(Enumerable.ToList));

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToListMethod;
}
