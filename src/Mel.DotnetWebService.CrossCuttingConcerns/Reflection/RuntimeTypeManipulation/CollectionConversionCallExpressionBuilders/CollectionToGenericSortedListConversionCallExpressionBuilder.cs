using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericSortedListConverter : CollectionConversionCallExpressionBuilder
{
	public static readonly CollectionToGenericSortedListConverter Instance = new();
	static SortedList<T,U> ToSortedList<T,U>(IEnumerable<KeyValuePair<T,U>> source)
		where T : notnull
	=> new SortedList<T,U>(source.ToDictionary());
	static readonly MethodInfo OpenGenericToSortedListMethod = typeof(CollectionToGenericSortedListConverter)
		.GetMethod(
			nameof(ToSortedList),
			BindingFlags.Static | BindingFlags.NonPublic)!;

	protected override MethodInfo GetClosedGenericToDestinationCollectionMethod(Type destinationCollectionType, Type destinationCollectionItemType)
	=> destinationCollectionItemType.IsKeyValuePairType(out var keyType, out var valueType)
		? OpenGenericToSortedListMethod.MakeGenericMethod(keyType, valueType)
		: throw new InvalidOperationException();

	protected override MethodCallExpression CallConversionToDestinationCollectionType(ParameterExpression sourceCollectionParameter, MethodInfo closedGenericToDestinationCollectionMethod)
	=> Expression.Call(
		closedGenericToDestinationCollectionMethod,
		sourceCollectionParameter);

	protected override MethodCallExpression CallSelectThenToDestinationCollectionType(ParameterExpression sourceCollectionParameter, MethodInfo closedGenericSelectMethod, LambdaExpression selectClauseContent, MethodInfo closedGenericToDestinationCollectionMethod)
	=> Expression.Call(
		closedGenericToDestinationCollectionMethod,
		Expression.Call(
			closedGenericSelectMethod,
			sourceCollectionParameter,
			selectClauseContent));
}
