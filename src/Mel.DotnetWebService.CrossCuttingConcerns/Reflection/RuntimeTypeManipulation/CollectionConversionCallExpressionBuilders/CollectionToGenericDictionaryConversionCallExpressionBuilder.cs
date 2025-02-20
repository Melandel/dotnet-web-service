using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericDictionaryConverter : CollectionConversionCallExpressionBuilder
{
	public static readonly CollectionToGenericDictionaryConverter Instance = new();
	static readonly MethodInfo OpenGenericToDictionaryMethod = EnumerableGenericMethods
		.Where(m => m.Name == nameof(Enumerable.ToDictionary))
		.Where(m => m.GetGenericArguments().Length == 2)
		.Single(m =>
		{
			var parameters = m.GetParameters();
			return parameters.Length == 1
				&& parameters[0].ParameterType.GetCollectionItemType().GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
		});

	protected override MethodInfo GetClosedGenericToDestinationCollectionMethod(Type destinationCollectionType, Type destinationCollectionItemType)
	=> destinationCollectionItemType.IsKeyValuePairType(out var keyType, out var valueType)
		? OpenGenericToDictionaryMethod.MakeGenericMethod(keyType, valueType)
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
