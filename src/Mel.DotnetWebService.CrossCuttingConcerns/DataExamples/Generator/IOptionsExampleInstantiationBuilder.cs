using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class IOptionsExampleInstantiationBuilder : ExampleInstantiationBuilder
{
	public static readonly IOptionsExampleInstantiationBuilder Instance = new();
	readonly MethodInfo OpenGenericCreateOptionsMethod = typeof(Options).GetMethod(nameof(Options.Create), BindingFlags.Public | BindingFlags.Static)!;

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var argType = type.GetGenericArguments().Single();
		return Expression.Call(
			OpenGenericCreateOptionsMethod.MakeGenericMethod(argType),
			ExampleInstantiationBuilder.BuildFor(argType, currentSalt, parentObjectsAlreadyRecursedThrough));
	}
}
