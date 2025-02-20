using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases.CollectionConversionTestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.RuntimeTypeManipulation;

class CollectionConverterShould
{
	const int SaltThatGuaranteesNoDuplicateItems = 1;
	object CreateCollectionWithNoDuplicateItems(Type collectionType)
	=> Some.Valid(collectionType, salt: SaltThatGuaranteesNoDuplicateItems);

	[TestCasesAggregation(typeof(CollectionConversionTestCases), nameof(CollectionConversionTestCases.AllTestCasesAggregatedIntoOne))]
	public void Foo(TestCasesAggregation<TestCase> aggregatedTestCases)	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var collection = CreateCollectionWithNoDuplicateItems(testCase.SourceType);

		// Act
		var converted = CollectionConverter.Convert(collection, testCase.DestinationType);

		// Assert
		Assert.That(converted, Is.AssignableTo(testCase.DestinationType), failingTestMessage);
		Assert.That(converted, Is.EquivalentTo(collection),               failingTestMessage);
	});

	[Test]
	public void Baz()
	{
		// Arrange
		var collection = Some.Valid<HashSet<int>>();
		var destinationType = typeof(SortedSet<int>);

		// Act
		var converted = CollectionConverter.Convert(collection, destinationType);

		// Assert
		Assert.That(converted, Is.AssignableTo(destinationType));
		Assert.That(converted, Is.EquivalentTo(collection.Distinct()));
	}

	[Test]
	public void BaBaz()
	{
		// Arrange
		var collection = Some.Valid< HashSet<KeyValuePair<int, int>>>();
		var destinationType = typeof(SortedDictionary<int, int>);

		// Act
		var converted = CollectionConverter.Convert(collection, destinationType);

		// Assert
		Assert.That(converted, Is.AssignableTo(destinationType));
		Assert.That(converted, Is.EquivalentTo(collection.Distinct()));
	}

	[Test]
	public void BeBaz()
	{
		// Arrange
		var collection = Some.Valid<HashSet<KeyValuePair<int, int>>>();
		var destinationType = typeof(SortedList<int, int>);

		// Act
		var converted = CollectionConverter.Convert(collection, destinationType);

		// Assert
		Assert.That(converted, Is.AssignableTo(destinationType));
		Assert.That(converted, Is.EquivalentTo(collection.Distinct()));
	}

	[TestCasesAggregation(typeof(CollectionConversionTestCases), nameof(CollectionConversionTestCases.DestinationTypeContainingEnumType))]
	public void Bar(TestCasesAggregation<TestCase> aggregatedTestCases)	=> aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var destinationType = testCase.DestinationType;
		var enumType = destinationType.GetCollectionItemType() switch
		{
			var destinationCollectionItemType when destinationCollectionItemType.IsKeyValuePairType(out var keyType, out _) => keyType,
			var destinationCollectionItemType => destinationCollectionItemType
		};
		IEnumerable collectionBase = testCase.SourceType.GetCollectionItemType() switch
		{
			var t when t == typeof(string) => Enum.GetNames(enumType),
			var t when t == typeof(int) => Enum.GetValues(enumType).Cast<int>().ToArray(),
			var t when t == typeof(KeyValuePair<string,string>) => Enum.GetNames(enumType).ToDictionary(str => str, str => str),
			var t when t == typeof(KeyValuePair<int,int>) => Enum.GetValues(enumType).Cast<int>().ToDictionary(i => i, i => i),
			_ => throw new NotImplementedException()
		};
		var collection = CollectionConverter.Convert(collectionBase, testCase.SourceType);

		// Act
		var converted = CollectionConverter.Convert(collection, destinationType);

		// Assert
		Assert.That(converted, Is.AssignableTo(destinationType), failingTestMessage);
		Assert.That(converted, Is.EquivalentTo(collection), failingTestMessage);
	});

	[TestCasesAggregation(typeof(CollectionConversionTestCases), nameof(CollectionConversionTestCases.TestCasesInvolvingNonGenericConstrainedCollections))]
	public void Convert_NonGenericConstrainedCollections(TestCasesAggregation<TestCase> aggregatedTestCases) => aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var collection = ConstrainedTypeInfos.TryGet(testCase.DestinationType, out var constrainedDestinationTypeInfo) && testCase.SourceType == constrainedDestinationTypeInfo.FullyNativeRootType
			? Some.Valid(testCase.SourceType, source: testCase.DestinationType)
			: CreateCollectionWithNoDuplicateItems(testCase.SourceType);

		// Act
		var converted = CollectionConverter.Convert(collection, testCase.DestinationType);

		// Assert
		Assert.That(converted, Is.AssignableTo(testCase.DestinationType), failingTestMessage);
		Assert.That(converted, Is.EquivalentTo(collection),               failingTestMessage);
	});

	[TestCasesAggregation(typeof(CollectionConversionTestCases), nameof(CollectionConversionTestCases.TestCasesInvolvingConstrainedCollectionsOfKeyValuePairs))]
	public void Convert_NonGenericConstrainedCollectionsOfKeyValuePairs(TestCasesAggregation<TestCase> aggregatedTestCases) => aggregatedTestCases.ForEach((testCase, failingTestMessage) =>
	{
		// Arrange
		var collection = CreateCollectionWithNoDuplicateItems(testCase.SourceType);

		// Act
		var converted = CollectionConverter.Convert(collection, testCase.DestinationType);

		// Assert
		Assert.That(converted, Is.AssignableTo(testCase.DestinationType), failingTestMessage);
		Assert.That(converted, Is.EquivalentTo(collection),               failingTestMessage);
	});
}
