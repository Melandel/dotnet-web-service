using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases.RuntimeCollectionBuilderTestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.RuntimeTypeManipulation;

class GenericListBuilderShould
{
	[TestCaseSource(typeof(RuntimeCollectionBuilderTestCases), nameof(RuntimeCollectionBuilderTestCases.AllTestCasesAggregatedIntoOne))]
	public void BuildListUsingAddOperation(TestCasesAggregation<TestCase> aggregatedTestCases)
	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var expectedCollection = Some.CollectionOf(testCase.CollectionItemType);

		// Act
		var listBuilder = GenericListBuilder.For(testCase.CollectionItemType);
		for (var i = 0; i < expectedCollection.Count; i++) { listBuilder.Add(expectedCollection[i]); }
		var list = listBuilder.BuildAsIList();

		// Assert
		Assert.That(list, Is.EquivalentTo(expectedCollection), failingTestMessage);
		Assert.That(list.GetStringRepresentation(), Is.EqualTo(expectedCollection.GetStringRepresentation()), failingTestMessage);
	});

	[TestCaseSource(typeof(RuntimeCollectionBuilderTestCases), nameof(RuntimeCollectionBuilderTestCases.AllTestCasesAggregatedIntoOne))]
	public void BuildListUsingAddRangeOperation(TestCasesAggregation<TestCase> aggregatedTestCases)
	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var expectedCollection = Some.CollectionOf(testCase.CollectionItemType);

		// Act
		var list = GenericListBuilder
			.For(testCase.CollectionItemType)
			.AddRange(expectedCollection)
			.BuildAsIList();

		// Assert
		Assert.That(list, Is.EquivalentTo(expectedCollection), failingTestMessage);
		Assert.That(list.GetStringRepresentation(), Is.EqualTo(expectedCollection.GetStringRepresentation()), failingTestMessage);
	});
}
