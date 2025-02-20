using System.Globalization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;


public class ExampleValueGeneratorTestCases
{
	public record TestCase(Type Type, string ExpectedStringRepresentationForSalt1);

	// 👇 All the test cases are aggregated into a single test case (in the sense of NUnit's list of individually runnable test)
	//   Justification: a high number of (NUnit) test cases exerts stress on VisualStudio, creating a 3-4 minutes freeze between each test suite run
	public static IEnumerable<TestCasesAggregation<TestCase>> AllTestCasesAggregatedIntoOne
	=> LeafObjectsAggregatedIntoSingleTestCase.Concat(ParentObjectsAggregatedIntoSingleTestCase);

	public static IEnumerable<TestCasesAggregation<TestCase>> LeafObjectsAggregatedIntoSingleTestCase
	{
		get
		{
			// 👇 Scalar value types
			yield return NativeValueTypes;
			yield return SystemTypesThatCanThrowOnInstanciation;
			yield return GenericTypes;
			yield return EnumTypes;
			yield return ConstrainedValueTypes;

		// 👇 Collection types
			yield return NativeCollectionTypes;
			yield return ConstrainedCollectionTypes;
			yield return CollectionOfCollectionsTypes;
			yield return CollectionOfKeyValuePairsTypes;

		// 👇 Collection of KeyValuePairs types
			yield return NativeKeyValuePairsTypes;
			yield return ConstrainedKeyValuePairsTypes;
			yield return KeyValuePairsOfCollectionTypes;
			yield return KeyValuePairsOfKeyValuePairTypes;

		// 👇 Structures holding data
			yield return FirstClassCollectionTypes;
			yield return FirstClassCollectionOfKeyValuePairsTypes;
			yield return PublicInstantiationOperationsExposingTypes;

			// 👇 Structures holding data
			yield return SingletonAccessExposingTypes;
			yield return InstancesAccessExposingTypes;
		}
	}

	public static IEnumerable<TestCasesAggregation<TestCase>> LeafObjectsExceptThoseInvolvingSingletonsAggregatedIntoSingleTestCase
	=> LeafObjectsAggregatedIntoSingleTestCase.Where(tc => tc != SingletonAccessExposingTypes);

	public static IEnumerable<TestCasesAggregation<TestCase>> ParentObjectsAggregatedIntoSingleTestCase
	=> LeafObjectsAggregatedIntoSingleTestCase.Select(
		testCasesAggregation => testCasesAggregation.Select(
			testCase => new TestCase(
				typeof(ClassArchetype.PositionalRecordTypeContainingGenericValue<>).MakeGenericType(testCase.Type),
				$"{{\"Value\":{testCase.ExpectedStringRepresentationForSalt1}}}")
		)
	);

	static readonly TestCasesAggregation<TestCase> NativeValueTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(int),            "1"),
		new(typeof(string),         "\"foo\""),
		new(typeof(Guid),           "\"00000000-0000-0000-0000-000000000001\""),
		new(typeof(DateTime),       "\"2000-02-01T21:20:19\""),
		new(typeof(DateTimeOffset), "\"2010-02-01T06:05:04+01:00\""),
		new(typeof(decimal),        "1"),
		new(typeof(double),         "1"),
		new(typeof(float),          "1"),
		new(typeof(byte),           "1"),
		new(typeof(long),           "1"),
		new(typeof(sbyte),          "1"),
		new(typeof(short),          "1"),
		new(typeof(uint),           "1"),
		new(typeof(ulong),          "1"),
		new(typeof(ushort),         "1"),
	]);

	static readonly TestCasesAggregation<TestCase> SystemTypesThatCanThrowOnInstanciation = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
			new(typeof(CultureInfo),    "\"fr-FR\""),
			new(typeof(RegionInfo),     "\"FR\""),
			new(typeof(Version),        "\"2.5.3\""),
			new(typeof(Uri),            "\"https://example.com/products?id=10\""),
	]);

	static readonly TestCasesAggregation<TestCase> ConstrainedValueTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(NonZeroInt),               "-24"),
		new(typeof(NonEmptyString),           "\"bar\""),
		new(typeof(NonEmptyGuid),             "\"00000000-0000-0000-0000-000000000002\""),
		new(typeof(NonDefaultDateTime),       "\"2026-03-02T18:17:16\""),
		new(typeof(NonDefaultDateTimeOffset), "\"2025-03-02T09:08:07-01:00\""),
		new(typeof(NonZeroDecimal),           "2.5"),
		new(typeof(NonZeroDouble),            "2.5"),
		new(typeof(NonZeroFloat),             "2.5"),
		new(typeof(NonZeroByte),              "24"),
		new(typeof(NonZeroLong),              "-24"),
		new(typeof(NonZeroSByte),             "-24"),
		new(typeof(NonZeroShort),             "-24"),
		new(typeof(NonZeroUInt),              "24"),
		new(typeof(NonZeroULong),             "24"),
		new(typeof(NonZeroUShort),            "24"),
	]);

	static readonly TestCasesAggregation<TestCase> ValueTypesConstrainedFurthermore = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(NonZeroInt),               "-24"),
		new(typeof(NonEmptyString),           "\"bar\""),
		new(typeof(NonEmptyGuid),             "\"00000000-0000-0000-0000-000000000002\""),
		new(typeof(NonDefaultDateTime),       "\"2026-03-02T18:17:16\""),
		new(typeof(NonDefaultDateTimeOffset), "\"2025-03-02T09:08:07-01:00\""),
		new(typeof(NonZeroDecimal),           "2.5"),
		new(typeof(NonZeroDouble),            "2.5"),
		new(typeof(NonZeroFloat),             "2.5"),
		new(typeof(NonZeroByte),              "24"),
		new(typeof(NonZeroLong),              "-24"),
		new(typeof(NonZeroSByte),             "-24"),
		new(typeof(NonZeroShort),             "-24"),
		new(typeof(NonZeroUInt),              "24"),
		new(typeof(NonZeroULong),             "24"),
		new(typeof(NonZeroUShort),            "24"),
	]);

	static readonly TestCasesAggregation<TestCase> GenericTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(KeyValuePair<int,string>),                                 "{\"Key\":1,\"Value\":\"foo\"}"),
		new(typeof(KeyValuePair<NonEmptyGuid,NonEmptyGuid>),                  "{\"Key\":\"00000000-0000-0000-0000-000000000002\",\"Value\":\"00000000-0000-0000-0000-000000000003\"}"),
		new(typeof(KeyValuePair<int[],NonEmptyArray<string>>),                "{\"Key\":[1,-24],\"Value\":[\"\",\"foo\"]}"),
		new(typeof(KeyValuePair<NonEmptyGuid[],NonEmptyArray<NonEmptyGuid>>), "{\"Key\":[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"],\"Value\":[\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\"]}"),
		new(typeof(MyGenericType<int>),                                       "{\"Value\":1}"),
	]);
	public class MyGenericType<T>
	{
		public T Value { get; }
		public MyGenericType(T value) => Value = value;
	}

	static readonly TestCasesAggregation<TestCase> EnumTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(ConsoleColor), "\"DarkGreen\""),
		new(typeof(      MyEnum), "\"FirstExposed\""),
	]);
	public enum MyEnum { TechnicalDefaultEnumValue = 0, FirstExposed = 1, MinusFirstExposed = -1, FourtyTwo = 42 }

	static readonly TestCasesAggregation<TestCase> NativeCollectionTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(            int[]),          "[1,-24]"),
		new(typeof(       List<int>) ,          "[1,-24]"),
		new(typeof(    HashSet<int>) ,          "[1,-24]"),
		new(typeof(IEnumerable<int>) ,          "[1,-24]"),
		new(typeof(      IList<int>) ,          "[1,-24]"),
		new(typeof(            NonEmptyGuid[]), "[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"]"),
		new(typeof(       List<NonEmptyGuid>),  "[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"]"),
		new(typeof(    HashSet<NonEmptyGuid>),  "[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"]"),
		new(typeof(IEnumerable<NonEmptyGuid>),  "[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"]"),
		new(typeof(      IList<NonEmptyGuid>),  "[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"]"),
		new(typeof(string[]),         "[\"foo\",\"bar\"]"),
		new(typeof(Guid[]),           "[\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\"]"),
		new(typeof(DateTime[]),       "[\"2000-02-01T21:20:19\",\"2026-03-02T18:17:16\"]"),
		new(typeof(DateTimeOffset[]), "[\"2010-02-01T06:05:04+01:00\",\"2025-03-02T09:08:07-01:00\"]"),
		new(typeof(decimal[]),        "[1,2.5]"),
		new(typeof(double[]),         "[1,2.5]"),
		new(typeof(float[]),          "[1,2.5]"),
		new(typeof(byte[]),           "\"ARg=\""),
		new(typeof(long[]),           "[1,-24]"),
		new(typeof(sbyte[]),          "[1,-24]"),
		new(typeof(short[]),          "[1,-24]"),
		new(typeof(uint[]),           "[1,24]"),
		new(typeof(ulong[]),          "[1,24]"),
		new(typeof(ushort[]),         "[1,24]"),
	]);

	static readonly TestCasesAggregation<TestCase> ConstrainedCollectionTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
			new(typeof(NonEmptyArray<int>),          "[0,1]"),
			new(typeof(NonEmptyArray<NonEmptyGuid>), "[\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\"]"),
			new(typeof(NonEmptyHashSet<int>),        "[0,1]"),
			new(typeof(NonEmptyLinkedList<int>),     "[0,1]"),
			new(typeof(NonEmptyList<int>),           "[0,1]"),
			new(typeof(NonEmptyQueue<int>),          "[0,1]"),
			new(typeof(NonEmptySortedSet<int>),      "[0,1]"),
			new(typeof(NonEmptyStack<int>),          "[0,1]"),
	]);

	static readonly TestCasesAggregation<TestCase> CollectionOfCollectionsTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
			new(typeof(                                                              int[][]),     "[[1,-24],[-24,60]]"),
			new(typeof(                                           List<              int[]>),      "[[1,-24],[-24,60]]"),
			new(typeof(                                           NonEmptyArray<List<int>>),       "[[0,1],[1,-24]]"),
			new(typeof(                               IEnumerable<NonEmptyArray<List<int>>>),      "[[[0,1],[1,-24]],[[0,1],[0,1]]]"),
			new(typeof(                               IEnumerable<NonEmptyArray<List<int>>>[]),    "[[[[0,1],[1,-24]],[[0,1],[0,1]]],[[[0,1],[0,1]],[[0,1]]]]"),
			new(typeof(                          List<IEnumerable<NonEmptyArray<List<int>>>[]>),   "[[[[[0,1],[1,-24]],[[0,1],[0,1]]],[[[0,1],[0,1]],[[0,1]]]],[[[[0,1],[0,1]],[[0,1]]],[[[0,1]],[[0,1],[1,-24]]]]]"),
			new(typeof(            NonEmptyArray<List<IEnumerable<NonEmptyArray<List<int>>>[]>>),  "[[[[[[0,1]],[[0,1],[1,-24]]],[[[0,1],[1,-24]],[[0,1],[0,1]]]],[[[[0,1],[1,-24]],[[0,1],[0,1]]],[[[0,1],[0,1]],[[0,1]]]]],[[[[[0,1],[1,-24]],[[0,1],[0,1]]],[[[0,1],[0,1]],[[0,1]]]],[[[[0,1],[0,1]],[[0,1]]],[[[0,1]],[[0,1],[1,-24]]]]]]"),
			new(typeof(IEnumerable<NonEmptyArray<List<IEnumerable<NonEmptyArray<List<int>>>[]>>>), "[[[[[[[0,1]],[[0,1],[1,-24]]],[[[0,1],[1,-24]],[[0,1],[0,1]]]],[[[[0,1],[1,-24]],[[0,1],[0,1]]],[[[0,1],[0,1]],[[0,1]]]]],[[[[[0,1],[1,-24]],[[0,1],[0,1]]],[[[0,1],[0,1]],[[0,1]]]],[[[[0,1],[0,1]],[[0,1]]],[[[0,1]],[[0,1],[1,-24]]]]]],[[[[[[0,1]],[[0,1],[1,-24]]],[[[0,1],[1,-24]],[[0,1],[0,1]]]],[[[[0,1],[1,-24]],[[0,1],[0,1]]],[[[0,1],[0,1]],[[0,1]]]]],[[[[[0,1]],[[0,1],[1,-24]]],[[[0,1],[1,-24]],[[0,1],[0,1]]]],[[[[0,1],[1,-24]],[[0,1],[0,1]]],[[[0,1],[0,1]],[[0,1]]]]]]]"),
	]);

	static readonly TestCasesAggregation<TestCase> NativeKeyValuePairsTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(Dictionary<int, int>),                   "{\"1\":1,\"-24\":-24}"),
		new(typeof(Dictionary<NonEmptyGuid, NonEmptyGuid>), "{\"00000000-0000-0000-0000-000000000002\":\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\":\"00000000-0000-0000-0000-000000000003\"}"),
	]);

	static readonly TestCasesAggregation<TestCase> ConstrainedKeyValuePairsTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(NonEmptyDictionary<int, int>),                   "{\"0\":0,\"1\":1}"),
		new(typeof(NonEmptyDictionary<NonEmptyGuid, NonEmptyGuid>), "{\"00000000-0000-0000-0000-000000000001\":\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\":\"00000000-0000-0000-0000-000000000002\"}"),
		new(typeof(NonEmptySortedDictionary<int, int>),             "{\"0\":0,\"1\":1}"),
		new(typeof(NonEmptySortedList<int, int>),                   "{\"0\":0,\"1\":1}"),
	]);

	static readonly TestCasesAggregation<TestCase> CollectionOfKeyValuePairsTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(              Dictionary<int, int>[]),   "[{\"1\":1,\"-24\":-24},{\"-24\":-24,\"60\":60}]"),
		new(typeof(         List<Dictionary<int, int>>),    "[{\"1\":1,\"-24\":-24},{\"-24\":-24,\"60\":60}]"),
		new(typeof(  IEnumerable<Dictionary<int, int>>),    "[{\"1\":1,\"-24\":-24},{\"-24\":-24,\"60\":60}]"),
		new(typeof(NonEmptyArray<Dictionary<int, int>>),    "[{\"0\":0,\"1\":1},{\"1\":1,\"-24\":-24}]"),
		new(typeof(              Dictionary<int, int>[][]), "[[{\"1\":1,\"-24\":-24},{\"-24\":-24,\"60\":60}],[{\"-24\":-24,\"60\":60},{\"60\":60,\"2147483647\":2147483647}]]"),
	]);

	static readonly TestCasesAggregation<TestCase> KeyValuePairsOfCollectionTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(        Dictionary<int[],              int[]>),              "{\"[1,-24]\":[1,-24],\"[-24,60]\":[-24,60]}"),
		new(typeof(        Dictionary<NonEmptyGuid[],     NonEmptyGuid[]>),     "{\"[\\\"00000000-0000-0000-0000-000000000002\\\",\\\"00000000-0000-0000-0000-000000000003\\\"]\":[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"],\"[\\\"00000000-0000-0000-0000-000000000003\\\",\\\"00000000-0000-0000-0000-000000000001\\\"]\":[\"00000000-0000-0000-0000-000000000003\",\"00000000-0000-0000-0000-000000000001\"]}"),
		new(typeof(NonEmptyDictionary<int[],              int[]>),              "{\"[0,1]\":[0,1],\"[1,-24]\":[1,-24]}"),
		new(typeof(NonEmptyDictionary<NonEmptyGuid[],     NonEmptyGuid[]>),     "{\"[\\\"00000000-0000-0000-0000-000000000001\\\",\\\"00000000-0000-0000-0000-000000000002\\\"]\":[\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\"],\"[\\\"00000000-0000-0000-0000-000000000002\\\",\\\"00000000-0000-0000-0000-000000000003\\\"]\":[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"]}"),
		new(typeof(        Dictionary<List<int>,          List<int>>),          "{\"[1,-24]\":[1,-24],\"[-24,60]\":[-24,60]}"),
		new(typeof(        Dictionary<IEnumerable<int>,   IEnumerable<int>>),   "{\"[1,-24]\":[1,-24],\"[-24,60]\":[-24,60]}"),
		new(typeof(        Dictionary<NonEmptyArray<int>, NonEmptyArray<int>>), "{\"[0,1]\":[0,1],\"[0,0]\":[0,0]}"),
	]);

	static readonly TestCasesAggregation<TestCase> KeyValuePairsOfKeyValuePairTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(        Dictionary<Dictionary<int,int>,                           Dictionary<int,int>>),                           "{\"{\\\"1\\\":1,\\\"-24\\\":-24}\":{\"1\":1,\"-24\":-24},\"{\\\"-24\\\":-24,\\\"60\\\":60}\":{\"-24\":-24,\"60\":60}}"),
		new(typeof(        Dictionary<Dictionary<NonEmptyGuid,NonEmptyGuid>,         Dictionary<NonEmptyGuid,NonEmptyGuid>>),         "{\"{\\\"00000000-0000-0000-0000-000000000002\\\":\\\"00000000-0000-0000-0000-000000000002\\\",\\\"00000000-0000-0000-0000-000000000003\\\":\\\"00000000-0000-0000-0000-000000000003\\\"}\":{\"00000000-0000-0000-0000-000000000002\":\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\":\"00000000-0000-0000-0000-000000000003\"},\"{\\\"00000000-0000-0000-0000-000000000003\\\":\\\"00000000-0000-0000-0000-000000000003\\\",\\\"00000000-0000-0000-0000-000000000001\\\":\\\"00000000-0000-0000-0000-000000000001\\\"}\":{\"00000000-0000-0000-0000-000000000003\":\"00000000-0000-0000-0000-000000000003\",\"00000000-0000-0000-0000-000000000001\":\"00000000-0000-0000-0000-000000000001\"}}"),
		new(typeof(        Dictionary<NonEmptyDictionary<int,int>,                   NonEmptyDictionary<int,int>>),                   "{\"{\\\"0\\\":0,\\\"1\\\":1}\":{\"0\":0,\"1\":1},\"{\\\"0\\\":0}\":{\"0\":0}}"),
		new(typeof(        Dictionary<NonEmptyDictionary<NonEmptyGuid,NonEmptyGuid>, NonEmptyDictionary<NonEmptyGuid,NonEmptyGuid>>), "{\"{\\\"00000000-0000-0000-0000-000000000001\\\":\\\"00000000-0000-0000-0000-000000000001\\\",\\\"00000000-0000-0000-0000-000000000002\\\":\\\"00000000-0000-0000-0000-000000000002\\\"}\":{\"00000000-0000-0000-0000-000000000001\":\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\":\"00000000-0000-0000-0000-000000000002\"},\"{\\\"00000000-0000-0000-0000-000000000001\\\":\\\"00000000-0000-0000-0000-000000000001\\\"}\":{\"00000000-0000-0000-0000-000000000001\":\"00000000-0000-0000-0000-000000000001\"}}"),
		new(typeof(NonEmptyDictionary<Dictionary<int,int>,                           Dictionary<int,int>>),                           "{\"{\\\"0\\\":0,\\\"1\\\":1}\":{\"0\":0,\"1\":1},\"{\\\"1\\\":1,\\\"-24\\\":-24}\":{\"1\":1,\"-24\":-24}}"),
		new(typeof(NonEmptyDictionary<Dictionary<NonEmptyGuid,NonEmptyGuid>,         Dictionary<NonEmptyGuid,NonEmptyGuid>>),         "{\"{\\\"00000000-0000-0000-0000-000000000001\\\":\\\"00000000-0000-0000-0000-000000000001\\\",\\\"00000000-0000-0000-0000-000000000002\\\":\\\"00000000-0000-0000-0000-000000000002\\\"}\":{\"00000000-0000-0000-0000-000000000001\":\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\":\"00000000-0000-0000-0000-000000000002\"},\"{\\\"00000000-0000-0000-0000-000000000002\\\":\\\"00000000-0000-0000-0000-000000000002\\\",\\\"00000000-0000-0000-0000-000000000003\\\":\\\"00000000-0000-0000-0000-000000000003\\\"}\":{\"00000000-0000-0000-0000-000000000002\":\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\":\"00000000-0000-0000-0000-000000000003\"}}"),
		new(typeof(NonEmptyDictionary<NonEmptyDictionary<int,int>,                   NonEmptyDictionary<int,int>>),                   "{\"{\\\"0\\\":0}\":{\"0\":0},\"{\\\"0\\\":0,\\\"1\\\":1}\":{\"0\":0,\"1\":1}}"),
		new(typeof(NonEmptyDictionary<NonEmptyDictionary<NonEmptyGuid,NonEmptyGuid>, NonEmptyDictionary<NonEmptyGuid,NonEmptyGuid>>), "{\"{\\\"00000000-0000-0000-0000-000000000001\\\":\\\"00000000-0000-0000-0000-000000000001\\\"}\":{\"00000000-0000-0000-0000-000000000001\":\"00000000-0000-0000-0000-000000000001\"},\"{\\\"00000000-0000-0000-0000-000000000001\\\":\\\"00000000-0000-0000-0000-000000000001\\\",\\\"00000000-0000-0000-0000-000000000002\\\":\\\"00000000-0000-0000-0000-000000000002\\\"}\":{\"00000000-0000-0000-0000-000000000001\":\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\":\"00000000-0000-0000-0000-000000000002\"}}"),
	]);

	static readonly TestCasesAggregation<TestCase> FirstClassCollectionTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(ClassArchetype.FirstClassCollectionOfValuesType),                                         "[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfArraysType),                                         "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfArraysConstructedFromListType),                      "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfArraysConstructedFromIEnumerableType),               "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfListsType),                                          "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfListsConstructedFromListType),                       "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfListsConstructedFromIEnumerableType),                "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfIReadOnlyCollectionsType),                           "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfIReadOnlyCollectionsConstructedFromListType),        "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(ClassArchetype.FirstClassCollectionOfIReadOnlyCollectionsConstructedFromIEnumerableType), "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
	]);

	static readonly TestCasesAggregation<TestCase> FirstClassCollectionOfKeyValuePairsTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(ClassArchetype.FirstClassCollectionOfKeyValuePairsType), "{\"-24\":\"bar\"}")
	]);


	static readonly TestCasesAggregation<TestCase> PublicInstantiationOperationsExposingTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(ClassArchetype.PositionalRecordTypeContainingValuePropertyType),                 "{\"IntProperty\":1,\"StringProperty\":\"foo\"}"),
		new(typeof(ClassArchetype.PositionalRecordTypeContainingConstrainedValuePropertyTypes),     "{\"IntProperty\":1,\"NonEmptyGuidProperty\":\"00000000-0000-0000-0000-000000000002\"}"),
		new(typeof(ClassArchetype.PositionalRecordTypeContainingParentObjectPropertyTypes),         "{\"RecordWithAnIntAndANonEmptyGuidProperty\":{\"IntProperty\":1,\"NonEmptyGuidProperty\":\"00000000-0000-0000-0000-000000000002\"}}"),
		new(typeof(ClassArchetype.MultiplePublicConstructorsHavingParametersExposingType),          "{\"BuiltFrom\":\"two-parameters constructor called with values (1,-24)\"}"),
		new(typeof(ClassArchetype.PublicParameterlessConstructorExposingType_Aka_GetSetStyleClass), "{\"String\":\"foo\",\"Int\":1,\"NonEmptyGuid\":\"00000000-0000-0000-0000-000000000002\"}"),
		new(typeof(ClassArchetype.SinglePublicStaticFactoryMethodExposingType),                     "{\"Value\":1}"),
		new(typeof(ClassArchetype.MultiplePublicStaticFactoryMethodsExposingType),                  "{\"BuiltFrom\":\"two-parameters static factory method called with values (1,-24)\"}"),
	]);

	static readonly TestCasesAggregation<TestCase> SingletonAccessExposingTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(ClassArchetype.PublicSingletonPropertyExposingType), "{}"),
		new(typeof(ClassArchetype.PublicSingletonFieldExposingType),    "{}"),
	]);

	static readonly TestCasesAggregation<TestCase> InstancesAccessExposingTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(ClassArchetype.PublicInstancesPropertyExposingType),      "{\"Name\":\"SecondExposed\",\"Value\":2}"),
		new(typeof(ClassArchetype.PublicInstancesReadonlyFieldExposingType), "{\"Name\":\"SecondExposed\",\"Value\":2}"),
		new(typeof(ClassArchetype.PublicInstancesFieldExposingType),         "{\"Name\":\"SecondExposed\",\"Value\":2}"),
	]);
}
