using System.Collections;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class ConstrainedTypeExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	public static readonly ConstrainedTypeExampleInstanciationBuilder Instance = new();
	ConstrainedTypeExampleInstanciationBuilder()
	{}
	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		if (!ConstrainedTypeInfos.TryGet(type, out var constrainedTypeInfo))
		{
			throw new InvalidOperationException($"{nameof(ConstrainedTypeExampleInstanciationBuilder)} could not {nameof(BuildInstanciationExpressionFor)}({type.GetName()});");
		}

		var rootTypeExampleValueCandidates = Expression.Property(Expression.Constant(constrainedTypeInfo), typeof(ConstrainedTypeInfo), nameof(ConstrainedTypeInfo.ValidValueExamples));
		var count = Expression.Property(rootTypeExampleValueCandidates, typeof(ICollection), nameof(ICollection.Count));
		var index = Expression.Modulo(currentSalt, count);
		var indexer = typeof(IList).GetProperty("Item");

		var rootTypeExampleValue = Expression.MakeIndex(rootTypeExampleValueCandidates, indexer, [index]);
		var staticFactoryMethod = constrainedTypeInfo.InvokableInstanciationMethod;
		var staticFactoryMethodInputType = staticFactoryMethod.GetParameters()[0].ParameterType;

		return Expression.Call(null, staticFactoryMethod, Expression.Convert(rootTypeExampleValue, staticFactoryMethodInputType));
	}
}
