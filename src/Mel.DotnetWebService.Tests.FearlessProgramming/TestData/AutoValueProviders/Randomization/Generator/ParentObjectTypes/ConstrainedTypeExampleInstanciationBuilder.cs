using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class ConstrainedTypeExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	public static readonly ConstrainedTypeExampleInstanciationBuilder Instance = new();
	public static readonly PropertyInfo ConstrainedTypeInfoValidValueExamplesProperty = typeof(ConstrainedTypeInfo).GetProperty(nameof(ConstrainedTypeInfo.ValidValueExamples));
	public static readonly PropertyInfo ICollectionCountProperty = typeof(ICollection).GetProperty(nameof(ICollection.Count));
	public static readonly PropertyInfo IListIndexer = typeof(IList).GetProperty("Item");
	ConstrainedTypeExampleInstanciationBuilder()
	{}
	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		if (!ConstrainedTypeInfos.TryGet(type, out var constrainedTypeInfo))
		{
			throw new InvalidOperationException($"{nameof(ConstrainedTypeExampleInstanciationBuilder)} could not {nameof(BuildInstanciationExpressionFor)}({type.GetName()});");
		}

		var rootTypeExampleValueCandidates = Expression.Property(Expression.Constant(constrainedTypeInfo), ConstrainedTypeInfoValidValueExamplesProperty);
		var staticFactoryMethod = constrainedTypeInfo.InvokableInstanciationMethod;
		var staticFactoryMethodInputType = staticFactoryMethod.GetParameters()[0].ParameterType;

		var index                = Expression.Variable(typeof(int),                  "index");
		var rootTypeExampleValue = Expression.Variable(staticFactoryMethodInputType, "rootTypeExampleValue");
		return Expression.Block(
			[index, rootTypeExampleValue],
			Expression.Assign(index,                Expression.Modulo(currentSalt, Expression.Property(rootTypeExampleValueCandidates, ICollectionCountProperty))),
			Expression.Assign(rootTypeExampleValue, Expression.Convert(Expression.MakeIndex(rootTypeExampleValueCandidates, IListIndexer, [index]), staticFactoryMethodInputType)),

			GenerativeSalts.Enqueue(generativeSalts, index),
			Expression.Call(null, staticFactoryMethod, Expression.Convert(rootTypeExampleValue, staticFactoryMethodInputType)));
	}
}
