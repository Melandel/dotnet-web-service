using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class QueueExampleInstanciationBuilder : SystemGenericCollectionExampleInstanciationBuilder
{
	public static readonly QueueExampleInstanciationBuilder Instance = new();
	protected override bool AllowsDuplicateElements => true;

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var itemType = type.GetCollectionItemType();
		var concreteType = typeof(Queue<>).MakeGenericType(itemType);
		var concreteTypeConstructor = concreteType.GetConstructor(Type.EmptyTypes)!;
		var concreteTypeAddMethod   = concreteType.GetMethod(nameof(Queue<object>.Enqueue), [itemType])!;

		var collectionVariable = Expression.Variable(concreteType, "queue");

		return Expression.Block(
			[collectionVariable],
			Expression.Assign(collectionVariable, Expression.New(concreteTypeConstructor)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, BuildFirstItemInstanciationExpression(itemType, currentSalt)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, BuildSecondItemInstanciationExpression(itemType, currentSalt)),
			collectionVariable
		);
	}
}
