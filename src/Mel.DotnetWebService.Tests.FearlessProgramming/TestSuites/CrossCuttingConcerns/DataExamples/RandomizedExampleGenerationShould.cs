using Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataExamples;

class RandomizedExampleGenerationShould
{
	[Test, Repeat(2)]
	public void BeAbleTo_Be_Regenerated()
	{
		var randomizedResult = Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<NonEmptyDictionary<ClassArchetype.NonEmptyGuids, NonEmptyList<StrictlyPositiveInt>>>>();
		var regeneratedValue = Regenerate.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<NonEmptyDictionary<ClassArchetype.NonEmptyGuids, NonEmptyList<StrictlyPositiveInt>>>>(
			randomizedResult.GenerativeSalts);

		var randomizedValue = randomizedResult.Value;
		var randomizedValueStr = randomizedValue.GetStringRepresentation();
		var regeneratedValueStr = regeneratedValue.GetStringRepresentation();

		Assert.That(
			randomizedValueStr,
			Is.EqualTo(regeneratedValueStr),
			string.Join(Environment.NewLine, [$"Expected {randomizedValueStr}", $"Got      {regeneratedValueStr}"]));
	}

	[Test, Retry(10 * RandomSalt.MeanRecurrenceIntervalForEmptyCollectionGeneration)]
	public void BeAbleTo_Be_Generate_EmptyCollections()
	=> Assert.That(
		Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<int[]>>().Value.Value,
		Is.Empty);
}
