using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization.Generator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataExamples;

class RandomizedExampleGenerationShould
{
	[Test, Repeat(2)]
	public void BeAbleTo_Be_Regenerated()
	{
		var randomizedValue = Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<NonEmptyDictionary<ClassArchetype.NonEmptyGuids, NonEmptyList<StrictlyPositiveInt>>>>(out var generativeSalts, out var regenerationHint);
		var regeneratedValue = Regenerate.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<NonEmptyDictionary<ClassArchetype.NonEmptyGuids, NonEmptyList<StrictlyPositiveInt>>>>(
			generativeSalts);

		var randomizedValueStr = randomizedValue.GetStringRepresentation();
		var regeneratedValueStr = regeneratedValue.GetStringRepresentation();

		Assert.That(
			regeneratedValueStr,
			Is.EqualTo(randomizedValueStr),
			string.Join(Environment.NewLine, [regenerationHint, $"Expected {randomizedValueStr}", $"Got      {regeneratedValueStr}"]));
	}

	[Test, Retry(10 * RandomSalt.MeanRecurrenceIntervalForEmptyCollectionGeneration)]
	public void BeAbleTo_Be_Generate_EmptyCollections()
	{
		var randomizedValue = Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<int[]>>(out var generativeSalts, out var regenerationHint);
		if (randomizedValue.Value != null && randomizedValue.Value.Length > 0)
		{
			Assert.Fail("This randomized value does not contain an empty collection - and we specifically want an empty collection here.");
		}

		var regeneratedValue = Regenerate.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<int[]>>(generativeSalts);
		var regeneratedValueStr = regeneratedValue.GetStringRepresentation();
		var randomizedValueStr = randomizedValue.GetStringRepresentation();

		Assert.That(
			regeneratedValueStr,
			Is.EqualTo(randomizedValueStr),
			string.Join(Environment.NewLine, [regenerationHint, $"Expected {randomizedValueStr}", $"Got      {regeneratedValueStr}"]));
	}

	[Test, Retry(10 * RandomSalt.MeanRecurrenceIntervalForNullReferenceGeneration)]
	public void BeAbleTo_Be_Generate_NullReferenceMembers()
	{
		var randomizedValue = Randomized.Valid<ClassArchetype.ContainingNullableProperties>(out var generativeSalts, out var regenerationHint);
		if (randomizedValue.PositiveInt is not null)
		{
			Assert.Fail("This randomized value does not contain a null reference - and we specifically want a null reference here.");
		}

		var regeneratedValue = Regenerate.Valid<ClassArchetype.ContainingNullableProperties>(generativeSalts);
		var regeneratedValueStr = regeneratedValue.GetStringRepresentation();
		var randomizedValueStr = randomizedValue.GetStringRepresentation();

		Assert.That(
			regeneratedValueStr,
			Is.EqualTo(randomizedValueStr),
			string.Join(Environment.NewLine, [regenerationHint, $"Expected {randomizedValueStr}", $"Got      {regeneratedValueStr}"]));
	}
}
