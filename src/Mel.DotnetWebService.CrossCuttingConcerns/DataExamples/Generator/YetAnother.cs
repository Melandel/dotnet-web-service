namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

static class YetAnother
{
	static readonly int SaltTwo = 2;
	public static T           ExampleOf<T>()          => ExampleValueGenerator.GenerateExampleOf<T>(   salt: SaltTwo);
	public static object      ExampleOf   (Type type) => ExampleValueGenerator.GenerateExampleOf(type, salt: SaltTwo);
}
