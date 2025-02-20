namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

static class Another
{
	static readonly int SaltOne = 1;
	public static T           ExampleOf<T>()          => ExampleValueGenerator.GenerateExampleOf<T>(   salt: SaltOne);
	public static object      ExampleOf   (Type type) => ExampleValueGenerator.GenerateExampleOf(type, salt: SaltOne);
}
