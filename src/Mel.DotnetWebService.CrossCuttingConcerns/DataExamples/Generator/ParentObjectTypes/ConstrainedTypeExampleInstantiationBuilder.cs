using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ConstrainedTypeExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	public static readonly ConstrainedTypeExampleInstantiationBuilder Instance = new();
	public static readonly PropertyInfo ConstrainedTypeInfoValidValueExamplesProperty = typeof(ConstrainedTypeInfo).GetProperty(nameof(ConstrainedTypeInfo.ValidValueExamples))!;
	public static readonly PropertyInfo ICollectionCountProperty = typeof(ICollection).GetProperty(nameof(ICollection.Count))!;
	public static readonly PropertyInfo IListIndexer = typeof(IList).GetProperty("Item")!;
	ConstrainedTypeExampleInstantiationBuilder()
	{}
	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		if (!ConstrainedTypeInfos.TryGet(type, out var constrainedTypeInfo))
		{
			throw new InvalidOperationException($"{nameof(ConstrainedTypeExampleInstantiationBuilder)} could not {nameof(BuildInstantiationExpressionFor)}({type.GetName()});");
		}

		var rootTypeExampleValueCandidates = Expression.Property(Expression.Constant(constrainedTypeInfo), ConstrainedTypeInfoValidValueExamplesProperty);
		var count = Expression.Property(rootTypeExampleValueCandidates, ICollectionCountProperty);
		var index = Expression.Modulo(currentSalt, count);

		var rootTypeExampleValue = Expression.MakeIndex(rootTypeExampleValueCandidates, IListIndexer, [index]);
		var staticFactoryMethod = constrainedTypeInfo.InvokableInstantiationMethod;
		var staticFactoryMethodInputType = staticFactoryMethod.GetParameters()[0].ParameterType;

		return Expression.Call(null, staticFactoryMethod, Expression.Convert(rootTypeExampleValue, staticFactoryMethodInputType));
	}
}
