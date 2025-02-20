using System.Collections.Concurrent;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

abstract class RandomizedExampleValueGeneratorUsingExpressionTrees
{
	static readonly ConcurrentDictionary<(Type, Type), CompiledInvokable> CachedExampleInstantiatorsByTypeAndExampleSourceType = [];
	static readonly ConcurrentDictionary<Type, CompiledInvokable> CachedExampleInstantiatorByType = [];
	internal abstract Expression GenerateInstanceOf(Type type);

	public static object GenerateExampleOf(Type type, Type exampleSourceType = null)
	{
		var instantiator = exampleSourceType is null
			? GetCachedOrBuildThenCacheExampleInstantiator(type)
			: GetCachedOrBuildThenCacheExampleInstantiator(type, exampleSourceType);

		return instantiator.Invoke();
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
		var exampleValue = Expression.Variable(type, "exampleValue");
		var generativeSalts = Expression.Variable(GenerativeSalts.Type, "generativeSalts");

		if (exampleSourceType == null)
		{
			var instantiator = CompiledInvokable.FromExpression(
				Expression.Block(
					[exampleValue, generativeSalts],
					RandomizedExampleInstantiationBuilder.BuildFor(type, exampleValue, generativeSalts),
					RandomizedExampleValueGenerationResult.Create(type, exampleValue, generativeSalts)));
			return instantiator;
		}
		else
		{
			var instantiationExpression = RandomizedExampleInstantiationBuilder.BuildFor(type, exampleValue, generativeSalts);
			var instantiationExpressionWithFinalType = ObjectConversionBuilder.BuildConversionExpressionFor(instantiationExpression.Type, type, instantiationExpression);
			var wrappedIntoResultObject = RandomizedExampleValueGenerationResult.Create(type, exampleValue, generativeSalts);

			var instantiator = CompiledInvokable.FromExpression(wrappedIntoResultObject);
			return instantiator;
		}
	}
}
