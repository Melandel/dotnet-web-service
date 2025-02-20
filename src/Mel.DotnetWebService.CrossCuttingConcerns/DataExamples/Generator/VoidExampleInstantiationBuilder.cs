using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class VoidExampleInstantiationBuilder : ExampleInstantiationBuilder
{
	public static readonly VoidExampleInstantiationBuilder Instance = new();

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	=> Expression.Empty();
}
