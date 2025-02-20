
using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.ConstrainedTypesLoading;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization.Regenerator;

public abstract class RandomizedExampleRegenerator
{
	public static T RegenerateRandomizedExampleOf<T>(Queue<int> generativeSalts) => (T) RegenerateRandomizedExampleOf(typeof(T), null, generativeSalts);

	public static T RegenerateRandomizedExampleOf<T>(Type exampleSourceType, Queue<int> generativeSalts) => (T) RegenerateRandomizedExampleOf(typeof(T), exampleSourceType, generativeSalts);

	public static T RegenerateRandomizedExampleOf<T, TExampleFrom>(Queue<int> generativeSalts) => (T) RegenerateRandomizedExampleOf(typeof(T), typeof(TExampleFrom), generativeSalts);

	public static object RegenerateRandomizedExampleOf(Type type, Type exampleSourceType, Queue<int> generativeSalts)
	{
		ConstrainedTypesLoader.LoadConstrainedTypesFromExecutingAssemblyIfNecessary();
		return RandomizedExampleRegeneratorUsingExpressionTrees.RegenerateExampleOf(type, exampleSourceType, generativeSalts);
	}
}
