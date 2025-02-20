using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

abstract class ExampleValueGeneratorUsingExpressionTrees
{
	static readonly Dictionary<Type, CompiledInvokable> CachedExampleInstanciatorByType = new();
	internal abstract Expression GenerateInstanceOf(Type type, int salt = 0);

	public static object GenerateExampleOf(Type type, int salt = 0)
	{
		if (!CachedExampleInstanciatorByType.TryGetValue(type, out var exampleInstanciator))
		{
			exampleInstanciator = BuildExampleInstanciator(type);
			CachedExampleInstanciatorByType.Add(type, exampleInstanciator);
		}

		return exampleInstanciator.Invoke(salt);
	}

	static CompiledInvokable BuildExampleInstanciator(Type type)
	{
		var saltParameter = Expression.Parameter(typeof(int), "salt");
		var instanciationExpression = ExampleInstanciationBuilder.BuildFor(type, saltParameter);

		var instanciator = CompiledInvokable.FromExpression(instanciationExpression, saltParameter);
		return instanciator;
	}
}
