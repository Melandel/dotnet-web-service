using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

class CollectionInstanciationExpressionBuilder : InstanciationExpressionBuilder
{
	public static readonly CollectionInstanciationExpressionBuilder Instance = new();
	CollectionInstanciationExpressionBuilder()
	{
	}

	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
	}
}
