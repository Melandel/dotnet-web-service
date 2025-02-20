using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericSortedDictionaryConverter : CollectionConversionCallExpressionBuilder
{
	public static readonly CollectionToGenericSortedDictionaryConverter Instance = new();
	static SortedDictionary<T,U> ToSortedDictionary<T,U>(IEnumerable<KeyValuePair<T,U>> source)
		where T : notnull
	=> new SortedDictionary<T,U>(source.ToDictionary());
	static readonly MethodInfo OpenGenericToSortedDictionaryMethod = typeof(CollectionToGenericSortedDictionaryConverter)
		.GetMethod(
			nameof(ToSortedDictionary),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo GetClosedGenericToDestinationCollectionMethod(Type destinationCollectionType, Type destinationCollectionItemType)
	=> destinationCollectionItemType.IsKeyValuePairType(out var keyType, out var valueType)
		? OpenGenericToSortedDictionaryMethod.MakeGenericMethod(keyType, valueType)
		: throw new InvalidOperationException();

	protected override MethodCallExpression CallConversionToDestinationCollectionType(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, MethodInfo closedGenericToDestinationCollectionMethod)
	=> Expression.Call(
		closedGenericToDestinationCollectionMethod,
		sourceCollectionParameter_OrExpressionBuiltSoFar);

	protected override MethodCallExpression CallSelectThenToDestinationCollectionType(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, MethodInfo closedGenericSelectMethod, LambdaExpression selectClauseLambda, MethodInfo closedGenericToDestinationCollectionMethod)
	=> Expression.Call(
		closedGenericToDestinationCollectionMethod,
		Expression.Call(
			closedGenericSelectMethod,
			sourceCollectionParameter_OrExpressionBuiltSoFar,
			selectClauseLambda));
}
