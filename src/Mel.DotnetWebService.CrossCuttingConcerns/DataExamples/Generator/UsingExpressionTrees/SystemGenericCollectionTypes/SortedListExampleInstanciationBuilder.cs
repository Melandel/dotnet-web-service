using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class SortedListExampleInstanciationBuilder : SystemGenericCollectionExampleInstanciationBuilder
{
	public static readonly SortedListExampleInstanciationBuilder Instance = new();
	protected override bool AllowsDuplicateElements => false;

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var kvpTypes = type.GetGenericArguments();
		var keyType = kvpTypes[0];
		var valueType = kvpTypes[1];
		var concreteType = typeof(SortedList<,>).MakeGenericType(keyType, valueType);
		var concreteTypeConstructor = concreteType.GetConstructor(Type.EmptyTypes)!;
		var concreteTypeAddMethod   = concreteType.GetMethod(nameof(SortedList<object, object>.Add))!;

		var collectionVariable = Expression.Variable(concreteType, "sortedList");
		var keyVariable1   = Expression.Variable(keyType,   "key1");
		var valueVariable1 = Expression.Variable(valueType, "value1");
		var keyVariable2   = Expression.Variable(keyType,   "key2");
		var valueVariable2 = Expression.Variable(valueType, "value2");

		return Expression.Block(
			[collectionVariable, keyVariable1, valueVariable1, keyVariable2, valueVariable2],
			Expression.Assign(keyVariable1,   BuildFirstItemInstanciationExpression(keyType,   currentSalt)),
			Expression.Assign(valueVariable1, BuildFirstItemInstanciationExpression(valueType, currentSalt)),
			Expression.Assign(keyVariable2,   BuildSecondItemInstanciationExpression(keyType,   currentSalt)),
			Expression.Assign(valueVariable2, BuildSecondItemInstanciationExpression(valueType, currentSalt)),
			Expression.Assign(collectionVariable, Expression.New(concreteTypeConstructor)),
			Expression.Call(collectionVariable, concreteTypeAddMethod, keyVariable1, valueVariable1),
			Expression.Call(collectionVariable, concreteTypeAddMethod, keyVariable2, valueVariable2),
			collectionVariable
		);
	}
}
