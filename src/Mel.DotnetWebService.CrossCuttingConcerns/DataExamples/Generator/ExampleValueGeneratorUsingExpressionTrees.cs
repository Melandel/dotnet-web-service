using System.Collections.Concurrent;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

abstract class ExampleValueGeneratorUsingExpressionTrees
{
	static readonly ConcurrentDictionary<(Type, Type), CompiledInvokable> CachedExampleInstantiatorsByTypeAndExampleSourceType = new();
	static readonly ConcurrentDictionary<Type, CompiledInvokable> CachedExampleInstantiatorByType = new();
	internal abstract Expression GenerateInstanceOf(Type type, int salt = 0);

	public static object GenerateExampleOf(Type type, Type? exampleSourceType = null, int salt = 0)
	{
		if (type.ContainsGenericParameters)
		{
			throw new NotSupportedException($"{nameof(ExampleValueGeneratorUsingExpressionTrees)}.{nameof(GenerateExampleOf)}() does not support types featuring unresolved generic parameters ({type.GetName()})");
		}

		var instantiator = exampleSourceType is null
			? GetCachedOrBuildThenCacheExampleInstantiator(type)
			: GetCachedOrBuildThenCacheExampleInstantiator(type, exampleSourceType);

		return instantiator.Invoke(salt);
	}

	static CompiledInvokable GetCachedOrBuildThenCacheExampleInstantiator(Type type)
	=> CachedExampleInstantiatorByType.GetOrAdd(type, _ => BuildExampleInstantiator(type));

	static CompiledInvokable GetCachedOrBuildThenCacheExampleInstantiator(Type type, Type exampleSourceType)
	{
		var cacheKey = (type, exampleSourceType);
		return CachedExampleInstantiatorsByTypeAndExampleSourceType.GetOrAdd(cacheKey, _ => BuildExampleInstantiator(type, exampleSourceType));
	}

	static CompiledInvokable BuildExampleInstantiator(Type type, Type? exampleSourceType = null)
	{
		var parentObjectsAlreadyRecursedThrough = new HashSet<Type>();
		var saltParameter = Expression.Parameter(typeof(int), "salt");

		if (exampleSourceType == null)
		{
			var instantiationExpression = ExampleInstantiationBuilder.BuildFor(type, saltParameter, parentObjectsAlreadyRecursedThrough);

			var instantiator = CompiledInvokable.FromExpression(instantiationExpression, saltParameter);
			return instantiator;
		}
		else
		{
			var instantiationExpression = ExampleInstantiationBuilder.BuildFor(exampleSourceType, saltParameter, parentObjectsAlreadyRecursedThrough);
			var instantiationExpressionWithFinalType = ObjectConversionBuilder.BuildConversionExpressionFor(instantiationExpression.Type, type, instantiationExpression);

			var instantiator = CompiledInvokable.FromExpression(instantiationExpressionWithFinalType, saltParameter);
			return instantiator;
		}
	}
}
