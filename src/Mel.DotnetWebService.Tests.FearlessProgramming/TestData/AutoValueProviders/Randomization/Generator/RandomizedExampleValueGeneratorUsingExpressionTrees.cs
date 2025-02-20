using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

abstract class RandomizedExampleValueGeneratorUsingExpressionTrees
{
	static readonly Dictionary<(Type, Type), CompiledInvokable> CachedExampleInstanciatorsByTypeAndExampleSourceType = new();
	static readonly Dictionary<Type, CompiledInvokable> CachedExampleInstanciatorByType = new();
	internal abstract Expression GenerateInstanceOf(Type type);

	public static object GenerateExampleOf(Type type, Type exampleSourceType = null)
	{
		var instanciator = exampleSourceType is null
			? GetCachedOrBuildThenCacheExampleInstanciator(type)
			: GetCachedOrBuildThenCacheExampleInstanciator(type, exampleSourceType);

		return instanciator.Invoke();
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
		var exampleValue = Expression.Variable(type, "exampleValue");
		var generativeSalts = Expression.Variable(GenerativeSalts.Type, "generativeSalts");

		if (exampleSourceType == null)
		{
			var instanciator = CompiledInvokable.FromExpression(
				Expression.Block(
					[exampleValue, generativeSalts],
					RandomizedExampleInstanciationBuilder.BuildFor(type, exampleValue, generativeSalts),
					RandomizedExampleValueGenerationResult.Create(type, exampleValue, generativeSalts)));
			return instanciator;
		}
		else
		{
			var instanciationExpression = RandomizedExampleInstanciationBuilder.BuildFor(type, exampleValue, generativeSalts);
			var instanciationExpressionWithFinalType = ObjectConversionBuilder.BuildConversionExpressionFor(instanciationExpression.Type, type, instanciationExpression);
			var wrappedIntoResultObject = RandomizedExampleValueGenerationResult.Create(type, exampleValue, generativeSalts);

			var instanciator = CompiledInvokable.FromExpression(wrappedIntoResultObject);
			return instanciator;
		}
	}
}
