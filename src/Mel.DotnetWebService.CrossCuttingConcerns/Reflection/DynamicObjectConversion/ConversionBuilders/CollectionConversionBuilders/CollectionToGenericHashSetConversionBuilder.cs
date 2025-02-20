using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

class CollectionToGenericHashSetConversionBuilder : SimpleCollectionConversionBuilder
{
	public static readonly CollectionToGenericHashSetConversionBuilder Instance = new();
	static readonly MethodInfo OpenGenericToHashSetMethod = EnumerableGenericMethods
		.Single(m => m.Name == nameof(Enumerable.ToHashSet) && m.GetParameters().Length == 1);

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToHashSetMethod;
}

