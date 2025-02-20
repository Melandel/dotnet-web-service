using System.Collections;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns;

class MutualizedTestsForExampleGenerationAndConstrainedTypes
{
	[Test]
	public void GeneratedExamples_Should_BeAbleTo_Be_Instantiated_Serialized_And_Deserialized_For_DeserializationFriendlyTypes()
	=> ExampleValueGeneratorTestCases.DeserializationFriendlyTestCasesAggregatedIntoOne
		.ParallelForEach((testCase, failingTestMessage) =>
		{
			// Serialization : Arrange
			var serialized = testCase.ExpectedStringRepresentationForSalt1;

			// Serialization : Act
			var deserialized = JsonSerializer.Deserialize(serialized, testCase.Type, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
			var reserialized = JsonSerializer.Serialize(deserialized, testCase.Type, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);

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
}
