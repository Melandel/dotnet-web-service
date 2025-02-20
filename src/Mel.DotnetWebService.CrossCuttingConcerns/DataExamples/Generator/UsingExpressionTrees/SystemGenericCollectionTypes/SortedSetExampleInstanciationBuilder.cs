using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class SortedSetExampleInstanciationBuilder : SystemGenericCollectionExampleInstanciationBuilder
{
	public static readonly SortedSetExampleInstanciationBuilder Instance = new();
	protected override bool AllowsDuplicateElements => false;

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var itemType = type.GetCollectionItemType();
		var concreteType = typeof(SortedSet<>).MakeGenericType(itemType);
		var concreteTypeConstructor = concreteType.GetConstructor(Type.EmptyTypes)!;
		var concreteTypeAddMethod   = concreteType.GetMethod(nameof(SortedSet<object>.Add), [itemType])!;

		var collectionVariable = Expression.Variable(concreteType, "sortedSet");

		return Expression.Block(
			[collectionVariable],
			Expression.Assign(collectionVariable, Expression.New(concreteTypeConstructor)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, BuildFirstItemInstanciationExpression(itemType, currentSalt)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, BuildSecondItemInstanciationExpression(itemType, currentSalt)),
			collectionVariable
		);
	}
}
