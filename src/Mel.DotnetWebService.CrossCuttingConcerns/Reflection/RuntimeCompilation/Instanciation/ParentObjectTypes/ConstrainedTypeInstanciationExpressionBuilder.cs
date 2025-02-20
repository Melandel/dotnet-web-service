using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

class ConstrainedTypeInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	public static readonly ConstrainedTypeInstanciationExpressionBuilder Instance = new();
	ConstrainedTypeInstanciationExpressionBuilder()
	{}
	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// if (ConstrainedTypeInfos.TryGet(type, out var constrainedTypeInfo))
		// {
		// 	var rootTypeExampleValue = constrainedTypeInfo.ValidValueExamples[salt % constrainedTypeInfo.ValidValueExamples.Count];
		// 	var methods = typeof(ConstrainedTypeInfo).GetMethods();
		// 	var method = methods.First(m => m.Name == nameof(ConstrainedTypeInfo.StaticFactoryMethod));
		// 	// It'd be cool to have access to the MethodCallExpression here
		// 	return Expression.Call(
		// 		Expression.Constant(constrainedTypeInfo),
		// 		method,
		// 		Expression.Constant(rootTypeExampleValue, constrainedTypeInfo.FullyNativeRootType));
		// }

		// if (ConstrainedTypeInfos.TryGet(type.GetGenericTypeDefinition(), out var constrainedGenericTypeInfo))
		// {
		// 	var rootTypeExampleValue = constrainedGenericTypeInfo.ValidValueExamples[salt % constrainedGenericTypeInfo.ValidValueExamples.Length];
		// 	//return constrainedTypeInfo.InvokeStaticFactoryMethod(rootTypeExampleValue);
		// 	return Expression.Call(
		// 		Expression.Constant(constrainedGenericTypeInfo),
		// 		typeof(ConstrainedTypeInfo).GetMethod(nameof(ConstrainedTypeInfo.StaticFactoryMethod)),
		// 		Expression.Constant(rootTypeExampleValue, constrainedTypeInfo.RootType));
		// }

		// throw new InvalidOperationException($"{nameof(ConstrainedTypeInstanciationExpressionBuilder)} could not {nameof(BuildInstanciationExpressionFor)}({type.GetName()}, {salt});");
	}
}
