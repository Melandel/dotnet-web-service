namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public abstract class ExampleValueGenerator
{
	public static T GenerateExampleOf<T>(int salt = 0) => (T) GenerateExampleOf(typeof(T), null, salt);

	public static T GenerateExampleOf<T>(Type exampleSourceType, int salt = 0) => (T) GenerateExampleOf(typeof(T), exampleSourceType, salt);

	public static T GenerateExampleOf<T, TExampleFrom>(int salt = 0) => (T) GenerateExampleOf(typeof(T), typeof(TExampleFrom), salt);

	public static object GenerateExampleOf(Type type, Type? exampleSourceType = null, int salt = 0)
	=> ExampleValueGeneratorUsingExpressionTrees.GenerateExampleOf(type, exampleSourceType, salt);
}
