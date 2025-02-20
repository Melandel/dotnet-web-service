using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.RuntimeTypeManipulation;

class GenericListBuilderShould
{
	[Test]
	public void BuildListUsingAddOperation()
	=> RuntimeCollectionBuilderTestCases.AllTestCasesAggregatedIntoOne
		.ForEach((testCase, failingTestMessage) =>
		{
			// Arrange
			var listType = typeof(List<>).MakeGenericType(testCase.CollectionItemType);
			var expectedCollection = Some.Valid(listType) as IList;

			// Act
			var listBuilder = GenericListBuilder.For(testCase.CollectionItemType);
			for (var i = 0; i < expectedCollection.Count; i++) { listBuilder.Add(expectedCollection[i]); }
			var list = listBuilder.BuildAsIList();

			// Assert
			Assert.That(list, Is.EquivalentTo(expectedCollection), failingTestMessage);
		});

	[Test]
	public void BuildListUsingAddRangeOperation()
	=> RuntimeCollectionBuilderTestCases.AllTestCasesAggregatedIntoOne
		.ForEach((testCase, failingTestMessage) =>
		{
			// Arrange
			var listType = typeof(List<>).MakeGenericType(testCase.CollectionItemType);
			var expectedCollection = Some.Valid(listType) as IList;

			// Act
			var list = GenericListBuilder
				.For(testCase.CollectionItemType)
				.AddRange(expectedCollection)
				.BuildAsIList();

			// Assert
			Assert.That(list, Is.EquivalentTo(expectedCollection), failingTestMessage);
		});
}
