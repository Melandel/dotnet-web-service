using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.ConstrainedTypesLoading;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration;

static class DeterministicExampleValueGenerator
{
	public static T GenerateExampleOf<T>(int salt = 0) => (T) GenerateExampleOf(typeof(T), null, salt);

	public static T GenerateExampleOf<T>(Type exampleSourceType, int salt = 0) => (T) GenerateExampleOf(typeof(T), exampleSourceType, salt);

	public static T GenerateExampleOf<T, TExampleFrom>(int salt = 0) => (T) GenerateExampleOf(typeof(T), typeof(TExampleFrom), salt);

	public static object GenerateExampleOf(Type type, Type exampleSourceType = null, int salt = 0)
	{
		ConstrainedTypesLoader.LoadConstrainedTypesFromExecutingAssemblyIfNecessary();
		return ExampleValueGenerator.GenerateExampleOf(type, exampleSourceType, salt);
	}
}
