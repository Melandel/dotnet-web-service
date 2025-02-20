namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public static class Example
{
	public static T Of<T>(int salt = 0) => ExampleValueGenerator.GenerateExampleOf<T>();
}
