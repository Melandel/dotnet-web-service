using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases.RuntimeCollectionBuilderTestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.RuntimeTypeManipulation;

class GenericArrayBuilderShould
{
	[TestCaseSource(typeof(RuntimeCollectionBuilderTestCases), nameof(RuntimeCollectionBuilderTestCases.AllTestCasesAggregatedIntoOne))]
	public void BuildArrayUsingAddOperation(TestCasesAggregation<TestCase> aggregatedTestCases)
	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var expectedCollection = Some.CollectionOf(testCase.CollectionItemType);

		// Act
		var arrayBuilder = GenericArrayBuilder.ForACapacityOf(2, testCase.CollectionItemType);
		for (var i = 0; i < expectedCollection.Count; i++) { arrayBuilder.Add(expectedCollection[i]); }
		var array = arrayBuilder.BuildAsIList();

		// Assert
		Assert.That(array, Is.EquivalentTo(expectedCollection), failingTestMessage);
	});

	[TestCaseSource(typeof(RuntimeCollectionBuilderTestCases), nameof(RuntimeCollectionBuilderTestCases.AllTestCasesAggregatedIntoOne))]
	public void BuildArrayUsingAddRangeOperation(TestCasesAggregation<TestCase> aggregatedTestCases)
	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var expectedCollection = Some.CollectionOf(testCase.CollectionItemType);

		// Act
		var array = GenericArrayBuilder
			.ForACapacityOf(2, testCase.CollectionItemType)
			.AddRange(expectedCollection)
			.BuildAsIList();

		// Assert
		Assert.That(array, Is.EquivalentTo(expectedCollection), failingTestMessage);
	});
}
