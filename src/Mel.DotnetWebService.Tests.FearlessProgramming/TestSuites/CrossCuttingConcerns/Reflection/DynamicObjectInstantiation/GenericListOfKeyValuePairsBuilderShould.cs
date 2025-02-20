using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectInstantiation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.Reflection.DynamicObjectInstantiation;

class GenericListOfKeyValuePairsBuilderShould
{
	[Test]
	public void BuildListOfKeyValuePairsUsingAddOperation()
	=> KeyValuePairBuilderTestCases.AllTestCasesAggregatedIntoOne
		.ParallelForEach((testCase, failingTestMessage) =>
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
