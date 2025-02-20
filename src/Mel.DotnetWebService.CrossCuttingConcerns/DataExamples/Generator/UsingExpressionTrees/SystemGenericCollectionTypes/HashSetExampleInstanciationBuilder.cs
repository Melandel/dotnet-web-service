using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class HashSetExampleInstanciationBuilder : SystemGenericCollectionExampleInstanciationBuilder
{
	public static readonly HashSetExampleInstanciationBuilder Instance = new();
	protected override bool AllowsDuplicateElements => false;

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var itemType = type.GetCollectionItemType();
		var concreteType = typeof(HashSet<>).MakeGenericType(itemType);
		var concreteTypeConstructor = concreteType.GetConstructor(Type.EmptyTypes)!;
		var concreteTypeAddMethod   = concreteType.GetMethod(nameof(HashSet<object>.Add))!;

		var collectionVariable = Expression.Variable(concreteType, "hashset");

		return Expression.Block(
			[collectionVariable],
			Expression.Assign(collectionVariable, Expression.New(concreteTypeConstructor)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, BuildFirstItemInstanciationExpression(itemType, currentSalt)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, BuildSecondItemInstanciationExpression(itemType, currentSalt)),
			collectionVariable
		);
	}
}
