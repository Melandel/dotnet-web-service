using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

public static class Some
{
	static readonly int SaltZero = 0;
	public static T           ExampleOf<T>()          => ExampleValueGenerator.GenerateExampleOf<T>(salt: SaltZero);
	public static T           ExampleOf<T>(int salt) => ExampleValueGenerator.GenerateExampleOf<T>(salt: SaltZero + salt);
	public static object      ExampleOf   (Type type) => ExampleValueGenerator.GenerateExampleOf(type, salt: SaltZero);
}
