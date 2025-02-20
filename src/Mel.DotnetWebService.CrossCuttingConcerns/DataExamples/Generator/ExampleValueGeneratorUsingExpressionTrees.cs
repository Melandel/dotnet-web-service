using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

abstract class ExampleValueGeneratorUsingExpressionTrees
{
	static readonly Dictionary<(Type, Type), CompiledInvokable> CachedExampleInstanciatorsByTypeAndExampleSourceType = new();
	static readonly Dictionary<Type, CompiledInvokable> CachedExampleInstanciatorByType = new();
	internal abstract Expression GenerateInstanceOf(Type type, int salt = 0);

	public static object GenerateExampleOf(Type type, Type? exampleSourceType = null, int salt = 0)
	{
		var instanciator = exampleSourceType is null
			? GetCachedOrBuildThenCacheExampleInstanciator(type)
			: GetCachedOrBuildThenCacheExampleInstanciator(type, exampleSourceType);

		return instanciator.Invoke(salt);
	}

	static CompiledInvokable GetCachedOrBuildThenCacheExampleInstanciator(Type type)
	{
		if (!CachedExampleInstanciatorByType.TryGetValue(type, out var exampleInstanciator))
		{
			exampleInstanciator = BuildExampleInstanciator(type);
			CachedExampleInstanciatorByType.Add(type, exampleInstanciator);
		}

		return exampleInstanciator;
	}

	static CompiledInvokable GetCachedOrBuildThenCacheExampleInstanciator(Type type, Type exampleSourceType)
	{
		var cacheKey = (type, exampleSourceType);
		if (!CachedExampleInstanciatorsByTypeAndExampleSourceType.TryGetValue(cacheKey, out var exampleInstanciator))
		{
			exampleInstanciator = BuildExampleInstanciator(type, exampleSourceType);
			CachedExampleInstanciatorsByTypeAndExampleSourceType.Add(cacheKey, exampleInstanciator);
		}

		return exampleInstanciator;
	}

	static CompiledInvokable BuildExampleInstanciator(Type type, Type? exampleSourceType = null)
	{
		if (exampleSourceType == null)
		{
			var saltParameter = Expression.Parameter(typeof(int), "salt");
			var instanciationExpression = ExampleInstanciationBuilder.BuildFor(type, saltParameter);

			var instanciator = CompiledInvokable.FromExpression(instanciationExpression, saltParameter);
			return instanciator;
		}
		else
		{
			var saltParameter = Expression.Parameter(typeof(int), "salt");
			var instanciationExpression = ExampleInstanciationBuilder.BuildFor(exampleSourceType, saltParameter);
			var instanciationExpressionWithFinalType = ObjectConversionBuilder.BuildConversionExpressionFor(instanciationExpression.Type, type, instanciationExpression);

			var instanciator = CompiledInvokable.FromExpression(instanciationExpressionWithFinalType, saltParameter);
			return instanciator;
		}
	}
}
