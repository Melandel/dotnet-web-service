using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericHashSetConverter : SimpleCollectionConversionLambdaExpressionBuilder
{
	public static readonly CollectionToGenericHashSetConverter Instance = new();
	static readonly MethodInfo OpenGenericToHashSetMethod = EnumerableGenericMethods
		.Single(m => m.Name == nameof(Enumerable.ToHashSet) && m.GetParameters().Length == 1);

	protected override MethodInfo OpenGenericToDestinationCollectionMethod => OpenGenericToHashSetMethod;
}

