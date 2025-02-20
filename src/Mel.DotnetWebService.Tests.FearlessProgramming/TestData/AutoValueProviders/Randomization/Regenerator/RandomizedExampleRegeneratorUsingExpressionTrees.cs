using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

abstract class RandomizedExampleRegeneratorUsingExpressionTrees
{
	static readonly Dictionary<(Type, Type), CompiledInvokable> CachedExampleInstanciatorsByTypeAndExampleSourceType = new();
	static readonly Dictionary<Type, CompiledInvokable> CachedExampleInstanciatorByType = new();

	public static object RegenerateExampleOf(Type type, Type exampleSourceType, Queue<int> generativeSalts)
	{
		var instanciator = exampleSourceType is null
			? GetCachedOrBuildThenCacheExampleInstanciator(type)
			: GetCachedOrBuildThenCacheExampleInstanciator(type, exampleSourceType);

		return instanciator.Invoke(generativeSalts);
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

	static CompiledInvokable BuildExampleInstanciator(Type type, Type exampleSourceType = null)
	{
		if (exampleSourceType == null)
		{
			var generativeSalts = Expression.Parameter(GenerativeSalts.Type, nameof(GenerativeSalts));
			var instanciationExpression = ExampleRegenerationBuilder.BuildFor(type, generativeSalts);

			var instanciator = CompiledInvokable.FromExpression(instanciationExpression, generativeSalts);
			return instanciator;
		}
		else
		{
			var generativeSaltsParameter = Expression.Parameter(GenerativeSalts.Type, nameof(GenerativeSalts));
			var instanciationExpression = ExampleRegenerationBuilder.BuildFor(exampleSourceType, generativeSaltsParameter);
			var instanciationExpressionWithFinalType = ObjectConversionBuilder.BuildConversionExpressionFor(instanciationExpression.Type, type, instanciationExpression);

			var instanciator = CompiledInvokable.FromExpression(instanciationExpressionWithFinalType, generativeSaltsParameter);
			return instanciator;
		}
	}
}
