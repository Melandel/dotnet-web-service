using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class DefaultConversionCallExpressionBuilder : CollectionConversionCallExpressionBuilder
{
	public static readonly DefaultConversionCallExpressionBuilder Instance = new();
	protected override MethodInfo GetClosedGenericToDestinationCollectionMethod(Type destinationCollectionType, Type destinationCollectionItemType)
	=> throw new NotImplementedException();

	protected override MethodCallExpression CallConversionToDestinationCollectionType(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, MethodInfo closedGenericToDestinationCollectionMethod)
	{
		throw new NotImplementedException();
	}

	protected override MethodCallExpression CallSelectThenToDestinationCollectionType(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, MethodInfo closedGenericSelectMethod, LambdaExpression selectClauseLambda, MethodInfo closedGenericToDestinationCollectionMethod)
	{
		throw new NotImplementedException();
	}

	// public object MyConvertCollection(object collection, Type destinationType)
	// {
	// 	var listType = typeof(List<>).MakeGenericType(collection.GetType().GetCollectionItemType());
	// 	object list = CollectionConversionCallExpressionBuilder.Convert(collection, listType);
	// 	var classWithMatchingSingleInstanciatorParameter = destinationType.CreateInstanceUsingConstructorOrFactoryMethod(list, BindingFlags.Public);
	// 	return classWithMatchingSingleInstanciatorParameter;
	// }
}
