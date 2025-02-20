using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases.KeyValuePairBuilderTestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.RuntimeTypeManipulation;

class GenericListOfKeyValuePairsBuilderShould
{
	[TestCaseSource(typeof(KeyValuePairBuilderTestCases), nameof(KeyValuePairBuilderTestCases.AllTestCasesAggregatedIntoOne))]
	public void BuildListOfKeyValuePairsUsingAddOperation(TestCasesAggregation<TestCase> aggregatedTestCases) => aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(int), testCase.ValueType);
		var expectedDictionary = Some.Valid(dictionaryType) as IDictionary;

		// Act
		var kvpsBuilder = GenericListOfKeyValuePairsBuilder.For(typeof(int), testCase.ValueType);
		foreach (DictionaryEntry entry in expectedDictionary) { kvpsBuilder.Add(entry.Key, entry.Value); }
		var kvps = kvpsBuilder.BuildAsIDictionary();

		// Assert
		Assert.That(kvps, Is.EquivalentTo(expectedDictionary), failingTestMessage);
	});
}
