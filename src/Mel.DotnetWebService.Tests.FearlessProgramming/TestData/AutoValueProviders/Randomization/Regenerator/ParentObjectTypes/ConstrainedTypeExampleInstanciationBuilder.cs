using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class ConstrainedTypeExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	public static readonly ConstrainedTypeExampleInstanciationBuilder Instance = new();
	public static readonly PropertyInfo ConstrainedTypeInfoValidValueExamplesProperty = typeof(ConstrainedTypeInfo).GetProperty(nameof(ConstrainedTypeInfo.ValidValueExamples));
	public static readonly PropertyInfo ICollectionCountProperty = typeof(ICollection).GetProperty(nameof(ICollection.Count));
	public static readonly PropertyInfo IListIndexer = typeof(IList).GetProperty("Item");
	ConstrainedTypeExampleInstanciationBuilder()
	{}
	public override Expression BuildInstanciationExpressionFor(Type type, ParameterExpression generativeSalts)
	{
		if (!ConstrainedTypeInfos.TryGet(type, out var constrainedTypeInfo))
		{
			throw new InvalidOperationException($"{nameof(ConstrainedTypeExampleInstanciationBuilder)} could not {nameof(BuildInstanciationExpressionFor)}({type.GetName()});");
		}

		var rootTypeExampleValueCandidates = Expression.Property(Expression.Constant(constrainedTypeInfo), ConstrainedTypeInfoValidValueExamplesProperty);
		var count = Expression.Property(rootTypeExampleValueCandidates, ICollectionCountProperty);
		var index = Expression.Modulo(GenerativeSalts.Dequeue(generativeSalts), count);

		var rootTypeExampleValue = Expression.MakeIndex(rootTypeExampleValueCandidates, IListIndexer, [index]);
		var staticFactoryMethod = constrainedTypeInfo.InvokableInstanciationMethod;
		var staticFactoryMethodInputType = staticFactoryMethod.GetParameters()[0].ParameterType;

		return Expression.Call(null, staticFactoryMethod, Expression.Convert(rootTypeExampleValue, staticFactoryMethodInputType));
	}
}
