namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

public abstract class RandomizedExampleValueGenerator
{
	public static RandomizedExampleValueGenerationResult<T> GenerateExampleOf<T>() => (RandomizedExampleValueGenerationResult<T>) GenerateExampleOf(typeof(T), null);

	public static RandomizedExampleValueGenerationResult<T> GenerateExampleOf<T>(Type exampleSourceType) => (RandomizedExampleValueGenerationResult<T>) GenerateExampleOf(typeof(T), exampleSourceType);

	public static RandomizedExampleValueGenerationResult<T> GenerateExampleOf<T, TExampleFrom>() => (RandomizedExampleValueGenerationResult<T>) GenerateExampleOf(typeof(T), typeof(TExampleFrom));

	public static object GenerateExampleOf(Type type, Type exampleSourceType = null)
	=> RandomizedExampleValueGeneratorUsingExpressionTrees.GenerateExampleOf(type, exampleSourceType);
}
