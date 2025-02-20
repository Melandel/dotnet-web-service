using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class VoidExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	public static readonly VoidExampleInstanciationBuilder Instance = new();

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	=> Expression.Empty();
}
