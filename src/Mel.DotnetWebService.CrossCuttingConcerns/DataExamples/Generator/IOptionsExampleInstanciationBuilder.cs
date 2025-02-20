using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class IOptionsExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	public static readonly IOptionsExampleInstanciationBuilder Instance = new();
	readonly MethodInfo OpenGenericCreateOptionsMethod = typeof(Options).GetMethod(nameof(Options.Create), BindingFlags.Public | BindingFlags.Static)!;

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var argType = type.GetGenericArguments().Single();
		return Expression.Call(
			OpenGenericCreateOptionsMethod.MakeGenericMethod(argType),
			ExampleInstanciationBuilder.BuildFor(argType, currentSalt));
	}
}
