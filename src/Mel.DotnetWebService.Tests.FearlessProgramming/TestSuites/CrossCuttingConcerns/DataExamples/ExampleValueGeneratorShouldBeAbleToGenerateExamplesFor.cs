using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Globalization.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.Serialization;

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

	[Test]
	public void AllTestCasesWithSalt1()
	=> ExampleValueGeneratorTestCases.DeserializationFriendlyTestCasesAggregatedIntoOne
		.ForEach((testCase, failingTestMessage) =>
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

	[Test]
	public void TestCasesThatAreNotSerializationFriendlyWithSalt1()
	=> ExampleValueGeneratorTestCases.DeserializationAdverseLeafObjectsAggregatedIntoSingleTestCase
		.ForEach((testCase, failingTestMessage) =>
		{
			var v = ExampleValueGeneratorTestCases.DeserializationAdverseLeafObjectsAggregatedIntoSingleTestCase;
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

	[Test]
	public void AllTestCasesWithDifferentOutputGivenDifferentLevelsOfSalt()
	=> ExampleValueGeneratorTestCases.AllTestCasesExceptThoseInvolvingSingletonsAggregatedIntoOne
		.ForEach((testCase, failingTestMessage) =>
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

	public void ParameterlessFuncDelegateTypes()
	{
		var parameterlessFunc0 = ExampleValueGenerator.GenerateExampleOf<Func<int>>();
		Assert.That(parameterlessFunc0.Invoke(), Is.EqualTo(ExampleValueGenerator.GenerateExampleOf<int>()));

		var parameterlessFunc1 = ExampleValueGenerator.GenerateExampleOf<Func<int>>(1);
		Assert.That(parameterlessFunc1.Invoke(), Is.EqualTo(ExampleValueGenerator.GenerateExampleOf<int>(1)));
	}

	public void FuncDelegateTypesWithParameters()
	{
		var parameterlessFunc0 = ExampleValueGenerator.GenerateExampleOf<Func<int, object, string>>();
		Assert.That(parameterlessFunc0.Invoke(0, null), Is.EqualTo(ExampleValueGenerator.GenerateExampleOf<string>()));

		var parameterlessFunc1 = ExampleValueGenerator.GenerateExampleOf<Func<int, object, string>>(1);
		Assert.That(parameterlessFunc1.Invoke(1, new object()), Is.EqualTo(ExampleValueGenerator.GenerateExampleOf<string>(1)));
	}

	public void ActionDelegateTypes()
	{
		var parameterlessAction = ExampleValueGenerator.GenerateExampleOf<Action>();
		Action action = () => parameterlessAction.Invoke();
		Assert.That(action, Throws.Nothing);

		var actionWithParameters = ExampleValueGenerator.GenerateExampleOf<Action<string>>();
		Action actionWithParams = () => actionWithParameters.Invoke(Some.Valid<string>());
		Assert.That(actionWithParams, Throws.Nothing);
	}
}
