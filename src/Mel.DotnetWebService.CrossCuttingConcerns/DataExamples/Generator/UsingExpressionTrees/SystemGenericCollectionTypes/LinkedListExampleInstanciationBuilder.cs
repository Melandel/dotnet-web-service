using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class LinkedListExampleInstanciationBuilder : SystemGenericCollectionExampleInstanciationBuilder
{
	public static readonly LinkedListExampleInstanciationBuilder Instance = new();
	protected override bool AllowsDuplicateElements => true;

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var itemType = type.GetCollectionItemType();
		var concreteType = typeof(LinkedList<>).MakeGenericType(itemType);
		var concreteTypeConstructor = concreteType.GetConstructor(Type.EmptyTypes)!;
		var concreteTypeAddMethod   = concreteType.GetMethod(nameof(LinkedList<object>.AddLast), [itemType])!;

		var collectionVariable = Expression.Variable(concreteType, "linkedList");

		return Expression.Block(
			[collectionVariable],
			Expression.Assign(collectionVariable, Expression.New(concreteTypeConstructor)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, BuildFirstItemInstanciationExpression(itemType, currentSalt)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, BuildSecondItemInstanciationExpression(itemType, currentSalt)),
			collectionVariable
		);
	}
}
