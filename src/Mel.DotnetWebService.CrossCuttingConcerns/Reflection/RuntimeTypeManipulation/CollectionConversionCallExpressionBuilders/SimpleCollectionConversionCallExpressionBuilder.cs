using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

abstract class SimpleCollectionConversionLambdaExpressionBuilder : CollectionConversionCallExpressionBuilder
{
	protected abstract MethodInfo OpenGenericToDestinationCollectionMethod { get; }
	protected override MethodInfo GetClosedGenericToDestinationCollectionMethod(Type destinationCollectionType, Type destinationCollectionItemType)
	=> OpenGenericToDestinationCollectionMethod.MakeGenericMethod(destinationCollectionItemType);

	protected override MethodCallExpression CallConversionToDestinationCollectionType(ParameterExpression sourceCollectionParameter, MethodInfo closedGenericToDestinationCollectionMethod)
	=> Expression.Call(closedGenericToDestinationCollectionMethod, sourceCollectionParameter);

	protected override MethodCallExpression CallSelectThenToDestinationCollectionType(ParameterExpression sourceCollectionParameter, MethodInfo closedGenericSelectMethod, LambdaExpression selectClauseContent, MethodInfo closedGenericToDestinationCollectionMethod)
	=> Expression.Call(
		closedGenericToDestinationCollectionMethod,
		Expression.Call(
			closedGenericSelectMethod,
			sourceCollectionParameter,
			selectClauseContent));
}
