using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class ArrayExampleInstanciationBuilder : SystemGenericCollectionExampleInstanciationBuilder
{
	public static readonly ArrayExampleInstanciationBuilder Instance = new();

	protected override bool AllowsDuplicateElements => true;

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var elementType = type.GetCollectionItemType();
		return Expression.NewArrayInit(
			elementType,
			BuildFirstItemInstanciationExpression(elementType, currentSalt),
			BuildSecondItemInstanciationExpression(elementType, currentSalt));
	}
}
