using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization.Regenerator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration;

static class Regenerate
{
	public static T Valid<T>(IEnumerable<int> generativeSalts)
	=> RandomizedExampleRegenerator.RegenerateRandomizedExampleOf<T>(new Queue<int>(generativeSalts));

	public static T Valid<T, TExampleFrom>(IEnumerable<int> generativeSalts)
	=> RandomizedExampleRegenerator.RegenerateRandomizedExampleOf<T, TExampleFrom>(new Queue<int>(generativeSalts));
}

