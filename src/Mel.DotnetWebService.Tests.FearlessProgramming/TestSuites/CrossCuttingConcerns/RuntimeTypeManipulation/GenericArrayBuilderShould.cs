using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.RuntimeTypeManipulation;

class GenericArrayBuilderShould
{
	[Test]
	public void BuildArrayUsingAddOperation()
	=> RuntimeCollectionBuilderTestCases.AllTestCasesAggregatedIntoOne
		.ForEach((testCase, failingTestMessage) =>
		{
			// Arrange
			var listType = typeof(List<>).MakeGenericType(testCase.CollectionItemType);
			var expectedCollection = Some.Valid(listType) as IList;

			// Act
			var arrayBuilder = GenericArrayBuilder.ForACapacityOf(2, testCase.CollectionItemType);
			for (var i = 0; i < expectedCollection.Count; i++) { arrayBuilder.Add(expectedCollection[i]); }
			var array = arrayBuilder.BuildAsIList();

			// Assert
			Assert.That(array, Is.EquivalentTo(expectedCollection), failingTestMessage);
		});

	[Test]
	public void BuildArrayUsingAddRangeOperation()
	=> RuntimeCollectionBuilderTestCases.AllTestCasesAggregatedIntoOne
		.ForEach((testCase, failingTestMessage) =>
		{
			// Arrange
			var listType = typeof(List<>).MakeGenericType(testCase.CollectionItemType);
			var expectedCollection = Some.Valid(listType) as IList;

			// Act
			var array = GenericArrayBuilder
				.ForACapacityOf(2, testCase.CollectionItemType)
				.AddRange(expectedCollection)
				.BuildAsIList();

			// Assert
			Assert.That(array, Is.EquivalentTo(expectedCollection), failingTestMessage);
		});
}
