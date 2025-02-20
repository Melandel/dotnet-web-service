using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases.ExampleValueGeneratorTestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataExamples;

class ExampleValueGeneratorShouldBeAbleToGenerateExamplesFor
{
	[TestCasesAggregation(typeof(ExampleValueGeneratorTestCases), nameof(ExampleValueGeneratorTestCases.AllTestCasesAggregatedIntoOne))]
	public void AllTestCasesWithSalt1(TestCasesAggregation<TestCase> aggregatedTestCases)
	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		if (testCase.Type == typeof(KeyValuePair<int[],NonEmptyArray<string>>))
		{
			var a = 0;
		}
		var v = ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 1).GetStringRepresentation();
		Assert.That(
			ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 1).GetStringRepresentation(),
			Is.EqualTo(testCase.ExpectedStringRepresentationForSalt1),
			string.Join(Environment.NewLine,
			[
				failingTestMessage,
				$"Expected {testCase.ExpectedStringRepresentationForSalt1}",
				$"Got      {v}"
			]));
	});

	[TestCasesAggregation(typeof(ExampleValueGeneratorTestCases), nameof(ExampleValueGeneratorTestCases.AllTestCasesAggregatedIntoOneExceptThoseInvolvingSingleton))]
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
		var instanciationExpression = InstanciationExpressionBuilder.BuildFor(testCase.Type, salt: 1);
		var compiledInvokable = CompiledInvokable.FromExpression(instanciationExpression);
		object instance = compiledInvokable.Invoke();
		var instanceStringRepresentation = instance.GetStringRepresentation();
		if (instanceStringRepresentation != testCase.ExpectedStringRepresentationForSalt1)
		{
			Console.WriteLine($"Expected {testCase.ExpectedStringRepresentationForSalt1}{Environment.NewLine}Got {instance}");
		}
		Assert.That(
			instanceStringRepresentation,
			Is.EqualTo(testCase.ExpectedStringRepresentationForSalt1),
			failingTestMessage);
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
