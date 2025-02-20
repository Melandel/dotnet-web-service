using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Globalization.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;
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
	[TestCasesAggregation(typeof(ExampleValueGeneratorTestCases), nameof(ExampleValueGeneratorTestCases.AllTestCasesAggregatedIntoOne))]
	public void AllTestCasesWithSalt1(TestCasesAggregation<TestCase> aggregatedTestCases)
	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		//if (testCase.Type != typeof(ClassArchetype.FirstClassCollectionOfKeyValuePairsType)) {  return; }
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

	[TestCasesAggregation(typeof(ExampleValueGeneratorTestCases), nameof(ExampleValueGeneratorTestCases.LeafObjectsExceptThoseInvolvingSingletonsAggregatedIntoSingleTestCase))]
	public void AllTestCasesWithDifferentOutputGivenDifferentLevelsOfSalt(TestCasesAggregation<TestCase> aggregatedTestCases)
	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		Assert.That(
			ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 0).GetStringRepresentation(),
			Is.Not.EqualTo(testCase.ExpectedStringRepresentationForSalt1),
			failingTestMessage);
	});

	[Ignore("todo")]
	[TestCasesAggregation(typeof(ExampleValueGeneratorTestCases), nameof(ExampleValueGeneratorTestCases.AllTestCasesAggregatedIntoOne))]
	public void ExampleValueGeneratorUsingExpressionTreesTest(TestCasesAggregation<TestCase> aggregatedTestCases)
	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var instanciationExpression = InstanciationExpressionBuilder.BuildFor(testCase.Type, salt: 1);
		var compiledInvokable = CompiledInvokable.FromExpression(instanciationExpression);

		// Act
		object instance = compiledInvokable.Invoke();

		// Assert
		var instanceStringRepresentation = instance.GetStringRepresentation();

		Assert.That(
			instanceStringRepresentation,
			Is.EqualTo(testCase.ExpectedStringRepresentationForSalt1),
			string.Join(Environment.NewLine,
			[
				failingTestMessage,
				$"Expected {testCase.ExpectedStringRepresentationForSalt1}",
				$"Got      {instanceStringRepresentation}"
			]));
	});

	// [Test]
	// public void ExampleValueGeneratorUsingExpressionTreesTest1()
	// {
	// 	Type type = typeof(KeyValuePair<NonEmptyGuid, NonEmptyGuid>);
	// 	var stringRepresentationForSalt1 = "{\"Key\":\"00000000-0000-0000-0000-000000000002\",\"Value\":\"00000000-0000-0000-0000-000000000003\"}";
	// 	var instanciationExpression = InstanciationExpressionBuilder.BuildFor(type, salt: 1);
	// 	var typedDelegateCreationOperation = typeof(Expression)
	// 		.GetMethods()
	// 		.First(mi => mi.Name == nameof(Expression.Lambda) && mi.IsGenericMethod && mi.GetParameters().Length == 2)
	// 		.MakeGenericMethod(typeof(Func<>)
	// 		.MakeGenericType(type));
	// 	dynamic typedDelegate = typedDelegateCreationOperation.Invoke(null, new object[] { instanciationExpression, Array.Empty<ParameterExpression>() });
	// 	dynamic func = typedDelegate.Compile();
	// 	//var func = Expression.Lambda<Func<object>>(Expression.Convert(instanciationExpression, type)).Compile();
	// 	object instance = func.Invoke();
	// 	var instanceStringRepresentation = instance.GetStringRepresentation();
	// 	if (instanceStringRepresentation != stringRepresentationForSalt1)
	// 	{
	// 		Console.WriteLine($"Expected {stringRepresentationForSalt1}{Environment.NewLine}Got {instance}");
	// 	}
	// 	Assert.That(instanceStringRepresentation, Is.EqualTo(stringRepresentationForSalt1));
	// }
}
