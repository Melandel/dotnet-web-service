using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

static class SelectClauseBuilder
{
	public static LambdaExpression Build(Type sourceCollectionItemType, Type destinationCollectionItemType)
	{
		var collectionItemParameterExpression = Expression.Parameter(sourceCollectionItemType, "x");
		return Expression.Lambda(
			ObjectConversionBuilder.BuildConversionExpressionFor(sourceCollectionItemType, destinationCollectionItemType, collectionItemParameterExpression),
			collectionItemParameterExpression);
	}
}
