using System.Collections.Concurrent;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

abstract class RandomizedExampleRegeneratorUsingExpressionTrees
{
	static readonly ConcurrentDictionary<(Type, Type), CompiledInvokable> CachedExampleInstantiatorsByTypeAndExampleSourceType = [];
	static readonly ConcurrentDictionary<Type, CompiledInvokable> CachedExampleInstantiatorByType = [];

	public static object RegenerateExampleOf(Type type, Type exampleSourceType, Queue<int> generativeSalts)
	{
		var instantiator = exampleSourceType is null
			? GetCachedOrBuildThenCacheExampleInstantiator(type)
			: GetCachedOrBuildThenCacheExampleInstantiator(type, exampleSourceType);

		return instantiator.Invoke(generativeSalts);
	}

	static CompiledInvokable GetCachedOrBuildThenCacheExampleInstantiator(Type type)
	=> CachedExampleInstantiatorByType.GetOrAdd(type, _ => BuildExampleInstantiator(type));

	static CompiledInvokable GetCachedOrBuildThenCacheExampleInstantiator(Type type, Type exampleSourceType)
	{
		var cacheKey = (type, exampleSourceType);
		return CachedExampleInstantiatorsByTypeAndExampleSourceType.GetOrAdd(cacheKey, _ => BuildExampleInstantiator(type, exampleSourceType));
	}

	static CompiledInvokable BuildExampleInstantiator(Type type, Type exampleSourceType = null)
	{
		if (exampleSourceType == null)
		{
			var generativeSalts = Expression.Parameter(GenerativeSalts.Type, nameof(GenerativeSalts));
			var instantiationExpression = ExampleRegenerationBuilder.BuildFor(type, generativeSalts);

			var instantiator = CompiledInvokable.FromExpression(instantiationExpression, generativeSalts);
			return instantiator;
		}
		else
		{
			var generativeSaltsParameter = Expression.Parameter(GenerativeSalts.Type, nameof(GenerativeSalts));
			var instantiationExpression = ExampleRegenerationBuilder.BuildFor(exampleSourceType, generativeSaltsParameter);
			var instantiationExpressionWithFinalType = ObjectConversionBuilder.BuildConversionExpressionFor(instantiationExpression.Type, type, instantiationExpression);

			var instantiator = CompiledInvokable.FromExpression(instantiationExpressionWithFinalType, generativeSaltsParameter);
			return instantiator;
		}
	}
}
