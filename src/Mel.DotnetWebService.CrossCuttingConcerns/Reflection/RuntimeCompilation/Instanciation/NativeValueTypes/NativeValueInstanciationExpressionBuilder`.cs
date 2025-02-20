using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class NativeValueInstanciationExpressionBuilder<T> : NativeValueInstanciationExpressionBuilder
{
	readonly ArrayOfUniqueValuesWithAtLeast2Items<T> ExampleValues;
	protected NativeValueInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<T> exampleValues)
	{
		ExampleValues = exampleValues;
	}

	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		return Expression.Constant(
			ExampleValues[salt % ExampleValues.Length],
			type);
	}
}
