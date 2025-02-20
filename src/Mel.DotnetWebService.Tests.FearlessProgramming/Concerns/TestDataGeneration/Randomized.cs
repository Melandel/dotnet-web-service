using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization;
using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization.Generator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration;

static class Randomized
{
	public static T Valid<T>()
	{
		var randomGenerationResult = RandomizedExampleValueGenerator.GenerateExampleOf<T>();
		StoreRandomizedGenerationResultInCaseOfFailingTest(randomGenerationResult);

		return randomGenerationResult.Value;
	}

	public static T Valid<T>(out Queue<int> generativeSalts)
	{
		var randomGenerationResult = RandomizedExampleValueGenerator.GenerateExampleOf<T>();
		StoreRandomizedGenerationResultInCaseOfFailingTest(randomGenerationResult);

		generativeSalts = randomGenerationResult.GenerativeSalts;
		return randomGenerationResult.Value;
	}

	public static T Valid<T>(out string regenerationHint)
	{
		var randomGenerationResult = RandomizedExampleValueGenerator.GenerateExampleOf<T>();
		StoreRandomizedGenerationResultInCaseOfFailingTest(randomGenerationResult);

		regenerationHint = BuildRegenerationHint(randomGenerationResult);
		return randomGenerationResult.Value;
	}

	public static T Valid<T>(out Queue<int> generativeSalts, out string regenerationHint)
	{
		var randomGenerationResult = RandomizedExampleValueGenerator.GenerateExampleOf<T>();
		StoreRandomizedGenerationResultInCaseOfFailingTest(randomGenerationResult);

		generativeSalts = randomGenerationResult.GenerativeSalts;
		regenerationHint = BuildRegenerationHint(randomGenerationResult);
		return randomGenerationResult.Value;
	}

	public static T Valid<T, TExampleFrom>()
	{
		var randomGenerationResult = RandomizedExampleValueGenerator.GenerateExampleOf<T, TExampleFrom>();
		StoreRandomizedGenerationResultInCaseOfFailingTest(randomGenerationResult);

		return randomGenerationResult.Value;
	}

	public static T Valid<T, TExampleFrom>(out Queue<int> generativeSalts)
	{
		var randomGenerationResult = RandomizedExampleValueGenerator.GenerateExampleOf<T, TExampleFrom>();
		StoreRandomizedGenerationResultInCaseOfFailingTest(randomGenerationResult);

		generativeSalts = randomGenerationResult.GenerativeSalts;
		return randomGenerationResult.Value;
	}

	public static T Valid<T, TExampleFrom>(out string regenerationHint)
	{
		var randomGenerationResult = RandomizedExampleValueGenerator.GenerateExampleOf<T, TExampleFrom>();
		StoreRandomizedGenerationResultInCaseOfFailingTest(randomGenerationResult);

		regenerationHint = BuildRegenerationHint(randomGenerationResult);
		return randomGenerationResult.Value;
	}

	public static T Valid<T, TExampleFrom>(out Queue<int> generativeSalts, out string regenerationHint)
	{
		var randomGenerationResult = RandomizedExampleValueGenerator.GenerateExampleOf<T, TExampleFrom>();
		StoreRandomizedGenerationResultInCaseOfFailingTest(randomGenerationResult);

		generativeSalts = randomGenerationResult.GenerativeSalts;
		regenerationHint = BuildRegenerationHint(randomGenerationResult);
		return randomGenerationResult.Value;
	}

	static void StoreRandomizedGenerationResultInCaseOfFailingTest<T>(RandomizedExampleValueGenerationResult<T> randomized)
	{
		if (TestContext.CurrentContext.Test.Properties.ContainsKey("Retry"))
		{
			return;
		}

		var feedback = new RandomizedTestDataFeedback(
			TestContext.CurrentContext.Test.ID,
			TestContext.CurrentContext.Test.Name,
			BuildRegenerationHint(randomized));

		RandomizedTestDataFeedbackProvider.StoreFeedbackInCaseOfTestFailure(feedback);
	}

	static RandomizedTestDataFeedback BuildRandomGenerationFeedback<T>(RandomizedExampleValueGenerationResult<T> randomized)
	{
		var testId = TestContext.CurrentContext.Test.ID;
		var testName = TestContext.CurrentContext.Test.Name;
		var feedback = BuildRegenerationHint(randomized);
		return new (testId, testName, feedback);
	}

	static string BuildRegenerationHint<T>(RandomizedExampleValueGenerationResult<T> randomized)
	{
		var regenerationSourceCode = $"{nameof(Regenerate)}.{nameof(Regenerate.Valid)}<{typeof(T).GetName()}>({randomized.GenerativeSalts.GetStringRepresentation()})";
		var separator = new string('-', regenerationSourceCode.Length);
		var value = randomized.Value.GetStringRepresentation(true);
		return string.Join(
			Environment.NewLine,
			[regenerationSourceCode, separator, value]);
	}
}
