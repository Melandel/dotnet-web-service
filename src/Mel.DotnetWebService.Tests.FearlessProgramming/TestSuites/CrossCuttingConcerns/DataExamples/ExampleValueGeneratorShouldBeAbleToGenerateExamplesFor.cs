using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Globalization.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.Serialization;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases.ExampleValueGeneratorTestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataExamples;

class ExampleValueGeneratorShouldBeAbleToGenerateExamplesFor
{
	static readonly JsonSerializerOptions OptionsInvolvingConstrainedTypesHandling;
	static ExampleValueGeneratorShouldBeAbleToGenerateExamplesFor()
	{
		OptionsInvolvingConstrainedTypesHandling = new JsonSerializerOptions()
		{
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};
		OptionsInvolvingConstrainedTypesHandling.Converters.Add(new KeyValuePairsWithComplexKeyTypeJsonConverter());
		OptionsInvolvingConstrainedTypesHandling.Converters.Add(new ConstrainedTypeJsonConverter());
		OptionsInvolvingConstrainedTypesHandling.Converters.Add(new JsonStringEnumConverter());
		OptionsInvolvingConstrainedTypesHandling.Converters.Add(new ConstrainedTypeJsonConverter());
		OptionsInvolvingConstrainedTypesHandling.Converters.Add(new TypeJsonConverter());
		OptionsInvolvingConstrainedTypesHandling.Converters.Add(new MethodInfoJsonConverter());
		OptionsInvolvingConstrainedTypesHandling.Converters.Add(new CultureInfoJsonConverter());
		OptionsInvolvingConstrainedTypesHandling.Converters.Add(new RegionInfoJsonConverter());
	}

	[TestCasesAggregation(typeof(ExampleValueGeneratorTestCases), nameof(ExampleValueGeneratorTestCases.DeserializationFriendlyTestCasesAggregatedIntoOne))]
	public void AllTestCasesWithSalt1(TestCasesAggregation<TestCase> aggregatedTestCases) => aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Serialization : Arrange
		var serialized = testCase.ExpectedStringRepresentationForSalt1;

		// Serialization : Act
		var deserialized = JsonSerializer.Deserialize(serialized, testCase.Type, OptionsInvolvingConstrainedTypesHandling);
		var reserialized = JsonSerializer.Serialize(deserialized, testCase.Type, OptionsInvolvingConstrainedTypesHandling);

		// Serialization : Assert
		var serializationFailingTestMessage = string.Join(Environment.NewLine,
		[
			$"[SERIALIZATION] {failingTestMessage}",
				$"Expected {serialized}",
				$"Got      {reserialized}"
		]);
		if (testCase.Type.FullName.Contains(nameof(Stack)))
		{
			Assert.That(
				reserialized.OrderBy(c => c),
				Is.EqualTo(serialized.OrderBy(c => c)),
				serializationFailingTestMessage);
		}
		else
		{
			Assert.That(
				reserialized,
				Is.EqualTo(serialized),
				serializationFailingTestMessage);
		}

		var serializationWorksCorrectly = serialized == reserialized;
		if (serializationWorksCorrectly)
		{
			// ExampleValueGeneration : Act
			var generatedExample = ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 1);

			// ExampleValueGeneration : Assert
			var exampleStringRepresentation = generatedExample.GetStringRepresentation();

			Assert.That(
				exampleStringRepresentation,
				Is.EqualTo(testCase.ExpectedStringRepresentationForSalt1),
				string.Join(Environment.NewLine,
				[
					$"[EXAMPLE VALUE GENERATION] {failingTestMessage}",
					$"Expected {testCase.ExpectedStringRepresentationForSalt1}",
					$"Got      {exampleStringRepresentation}"
				]));
		}
	});

	[TestCasesAggregation(typeof(ExampleValueGeneratorTestCases), nameof(ExampleValueGeneratorTestCases.DeserializationAdverseLeafObjectsAggregatedIntoSingleTestCase))]
	public void TestCasesThatAreNotSerializationFriendlyWithSalt1(TestCasesAggregation<TestCase> aggregatedTestCases) => aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Act
		var generatedExample = ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 1);

		// Assert
		var exampleStringRepresentation = generatedExample.GetStringRepresentation();

		Assert.That(
			exampleStringRepresentation,
			Is.EqualTo(testCase.ExpectedStringRepresentationForSalt1),
			string.Join(Environment.NewLine,
			[
				$"[EXAMPLE VALUE GENERATION] {failingTestMessage}",
				$"Expected {testCase.ExpectedStringRepresentationForSalt1}",
				$"Got      {exampleStringRepresentation}"
			]));
	});

	[TestCasesAggregation(typeof(ExampleValueGeneratorTestCases), nameof(ExampleValueGeneratorTestCases.AllTestCasesExceptThoseInvolvingSingletonsAggregatedIntoOne))]
	public void AllTestCasesWithDifferentOutputGivenDifferentLevelsOfSalt(TestCasesAggregation<TestCase> aggregatedTestCases) => aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		var salt0 = ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 0).GetStringRepresentation();
		var salt1 = testCase.ExpectedStringRepresentationForSalt1;
		var salt2 = ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 2).GetStringRepresentation();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(salt0, Is.Not.EqualTo(salt1), failingTestMessage);
			Assert.That(salt0, Is.Not.EqualTo(salt2), failingTestMessage);
			Assert.That(salt1, Is.Not.EqualTo(salt2), failingTestMessage);
		}
	});
}
