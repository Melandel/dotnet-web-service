namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

public abstract class RandomizedExampleRegenerator
{
	public static T RegenerateRandomizedExampleOf<T>(Queue<int> generativeSalts) => (T) RegenerateRandomizedExampleOf(typeof(T), null, generativeSalts);

	public static T RegenerateRandomizedExampleOf<T>(Type exampleSourceType, Queue<int> generativeSalts) => (T) RegenerateRandomizedExampleOf(typeof(T), exampleSourceType, generativeSalts);

	public static T RegenerateRandomizedExampleOf<T, TExampleFrom>(Queue<int> generativeSalts) => (T) RegenerateRandomizedExampleOf(typeof(T), typeof(TExampleFrom), generativeSalts);

	public static object RegenerateRandomizedExampleOf(Type type, Type exampleSourceType, Queue<int> generativeSalts)
	=> RandomizedExampleRegeneratorUsingExpressionTrees.RegenerateExampleOf(type, exampleSourceType, generativeSalts);
}
