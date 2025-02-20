namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration;

static class YetAnother
{
	static readonly int SaltLevel = 2;

	public static T Valid<T>(int salt = 0)
	=> DeterministicExampleValueGenerator.GenerateExampleOf<T>(SaltLevel + salt);

	public static T Valid<T, TExampleFrom>(int salt = 0)
	=> DeterministicExampleValueGenerator.GenerateExampleOf<T, TExampleFrom>(SaltLevel + salt);

	public static object Valid(Type type, int salt = 0)
	=> DeterministicExampleValueGenerator.GenerateExampleOf(type, null, SaltLevel + salt);

	public static object Valid(Type type, Type source, int salt = 0)
	=> DeterministicExampleValueGenerator.GenerateExampleOf(type, source, SaltLevel + salt);
}
