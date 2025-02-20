using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericConstrainedCollectionConversionCallExpressionBuilder : CollectionConversionCallExpressionBuilder
{
	public static readonly CollectionToGenericConstrainedCollectionConversionCallExpressionBuilder Instance = new();
	protected override MethodInfo GetClosedGenericToDestinationCollectionMethod(Type destinationCollectionType, Type destinationCollectionItemType)
	=> ConstrainedTypeInfos.TryGet(destinationCollectionType, out var constrainedTypeInfo)
		? constrainedTypeInfo.InvokableInstanciationMethod
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

