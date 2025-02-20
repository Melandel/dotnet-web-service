namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

static class Some
{
	static readonly int SaltZero = 0;
	public static T           ExampleOf<T>()          => ExampleValueGenerator.GenerateExampleOf<T>(salt: SaltZero);
	public static object      ExampleOf   (Type type) => ExampleValueGenerator.GenerateExampleOf(type, salt: SaltZero);
}
