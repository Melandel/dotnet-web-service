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
	{
		get
		{
			// 👇 Scalar value types
			yield return NativeValueTypes;
			yield return SystemTypesThatCanThrowOnInstanciation;
			yield return GenericTypes;
			yield return EnumTypes;

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

	public static IEnumerable<TestCasesAggregation<TestCase>> AllTestCasesAggregatedIntoOneExceptThoseInvolvingSingleton
	=> AllTestCasesAggregatedIntoOne.Where(tc => tc != SingletonAccessExposingTypes);

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
		new(typeof(FirstClassCollectionType),                                                 "[\"00000000-0000-0000-0000-000000000002\",\"00000000-0000-0000-0000-000000000003\"]"),
		new(typeof(FirstClassCollectionOfArraysType),                                         "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(FirstClassCollectionOfArraysConstructedFromListType),                      "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(FirstClassCollectionOfArraysConstructedFromIEnumerableType),               "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(FirstClassCollectionOfListsType),                                          "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(FirstClassCollectionOfListsConstructedFromListType),                       "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(FirstClassCollectionOfListsConstructedFromIEnumerableType),                "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(FirstClassCollectionOfIReadOnlyCollectionsType),                           "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(FirstClassCollectionOfIReadOnlyCollectionsConstructedFromListType),        "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
		new(typeof(FirstClassCollectionOfIReadOnlyCollectionsConstructedFromIEnumerableType), "[[\"00000000-0000-0000-0000-000000000002\"],[\"00000000-0000-0000-0000-000000000003\"]]"),
	]);
	public record PositionalRecordTypeContainingValuePropertyType(int IntProperty, string StringProperty);
	public record PositionalRecordTypeContainingConstrainedValuePropertyTypes(int IntProperty, NonEmptyGuid NonEmptyGuidProperty);
	public record PositionalRecordTypeContainingParentObjectPropertyTypes(PositionalRecordTypeContainingConstrainedValuePropertyTypes RecordWithAnIntAndANonEmptyGuidProperty);
	public sealed class FirstClassCollectionType : ConstrainedCollection<NonEmptyGuid[]>, IConstrainedCollection<Guid, FirstClassCollectionType>
	{
		FirstClassCollectionType(NonEmptyGuid[] value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ Some.Value<NonEmptyGuid>() ],
				[ Another.Value<NonEmptyGuid>(), YetAnother.Value<NonEmptyGuid>() ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid>>([ Guid.Empty ], "Value must not be empty") ]);

		public static FirstClassCollectionType ApplyConstraintsTo(IEnumerable<Guid> collection)
		{
			try
			{
				return new(collection.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfArraysType : ConstrainedCollection<NonEmptyGuid[][]>, IConstrainedCollection<Guid[], FirstClassCollectionOfArraysType>
	{
		FirstClassCollectionOfArraysType(NonEmptyGuid[][] value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid[]>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid[]>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfArraysType ApplyConstraintsTo(IEnumerable<Guid[]> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfArraysType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfArraysType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfArraysConstructedFromListType : ConstrainedCollection<NonEmptyGuid[][]>, IConstrainedCollection<List<Guid>, FirstClassCollectionOfArraysConstructedFromListType>
	{
		FirstClassCollectionOfArraysConstructedFromListType(NonEmptyGuid[][] value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<List<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<List<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfArraysConstructedFromListType ApplyConstraintsTo(IEnumerable<List<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfArraysConstructedFromListType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfArraysConstructedFromListType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfArraysConstructedFromIEnumerableType : ConstrainedCollection<NonEmptyGuid[][]>, IConstrainedCollection<IEnumerable<Guid>, FirstClassCollectionOfArraysConstructedFromIEnumerableType>
	{
		FirstClassCollectionOfArraysConstructedFromIEnumerableType(NonEmptyGuid[][] value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<IEnumerable<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<IEnumerable<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfArraysConstructedFromIEnumerableType ApplyConstraintsTo(IEnumerable<IEnumerable<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfArraysConstructedFromIEnumerableType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfArraysConstructedFromIEnumerableType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfListsType : ConstrainedCollection<List<List<NonEmptyGuid>>>, IConstrainedCollection<Guid[], FirstClassCollectionOfListsType>
	{
		FirstClassCollectionOfListsType(List<List<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid[]>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid[]>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfListsType ApplyConstraintsTo(IEnumerable<Guid[]> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToList()).ToList());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfListsType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfListsType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfListsConstructedFromListType : ConstrainedCollection<List<List<NonEmptyGuid>>>, IConstrainedCollection<List<Guid>, FirstClassCollectionOfListsConstructedFromListType>
	{
		FirstClassCollectionOfListsConstructedFromListType(List<List<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<List<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<List<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfListsConstructedFromListType ApplyConstraintsTo(IEnumerable<List<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToList()).ToList());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfListsConstructedFromListType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfListsConstructedFromListType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfListsConstructedFromIEnumerableType : ConstrainedCollection<List<List<NonEmptyGuid>>>, IConstrainedCollection<IEnumerable<Guid>, FirstClassCollectionOfListsConstructedFromIEnumerableType>
	{
		FirstClassCollectionOfListsConstructedFromIEnumerableType(List<List<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<IEnumerable<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<IEnumerable<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfListsConstructedFromIEnumerableType ApplyConstraintsTo(IEnumerable<IEnumerable<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToList()).ToList());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfListsConstructedFromIEnumerableType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfListsConstructedFromIEnumerableType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfIReadOnlyCollectionsType : ConstrainedCollection<IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>>>, IConstrainedCollection<Guid[], FirstClassCollectionOfIReadOnlyCollectionsType>
	{
		FirstClassCollectionOfIReadOnlyCollectionsType(IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid[]>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid[]>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfIReadOnlyCollectionsType ApplyConstraintsTo(IEnumerable<Guid[]> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfIReadOnlyCollectionsType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfIReadOnlyCollectionsType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfIReadOnlyCollectionsConstructedFromListType : ConstrainedCollection<IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>>>, IConstrainedCollection<List<Guid>, FirstClassCollectionOfIReadOnlyCollectionsConstructedFromListType>
	{
		FirstClassCollectionOfIReadOnlyCollectionsConstructedFromListType(IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<List<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<List<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfIReadOnlyCollectionsConstructedFromListType ApplyConstraintsTo(IEnumerable<List<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfIReadOnlyCollectionsConstructedFromListType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfIReadOnlyCollectionsConstructedFromListType>(developerMistake, collection);
			}
		}
	}
	public sealed class FirstClassCollectionOfIReadOnlyCollectionsConstructedFromIEnumerableType : ConstrainedCollection<IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>>>, IConstrainedCollection<IEnumerable<Guid>, FirstClassCollectionOfIReadOnlyCollectionsConstructedFromIEnumerableType>
	{
		FirstClassCollectionOfIReadOnlyCollectionsConstructedFromIEnumerableType(IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<IEnumerable<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<IEnumerable<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfIReadOnlyCollectionsConstructedFromIEnumerableType ApplyConstraintsTo(IEnumerable<IEnumerable<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfIReadOnlyCollectionsConstructedFromIEnumerableType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfIReadOnlyCollectionsConstructedFromIEnumerableType>(developerMistake, collection);
			}
		}
	}

	static readonly TestCasesAggregation<TestCase> FirstClassCollectionOfKeyValuePairsTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(FirstClassCollectionOfKeyValuePairsType), "{\"-24\":\"bar\"}")
	]);
	public sealed class FirstClassCollectionOfKeyValuePairsType : ConstrainedCollectionOfKeyValuePairs<NonEmptyDictionary<NonZeroInt, NonEmptyString>>, IConstrainedCollectionOfKeyValuePairs<NonZeroInt, NonEmptyString, FirstClassCollectionOfKeyValuePairsType>
	{
		public FirstClassCollectionOfKeyValuePairsType(NonEmptyDictionary<NonZeroInt, NonEmptyString> collectionOfKeyValuePairs) : base(collectionOfKeyValuePairs)
		{ }

		public static ExampleValues<IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ KeyValuePair.Create(Some.Value<NonZeroInt>(), Some.Value<NonEmptyString>()) ],
				[ KeyValuePair.Create(Another.Value<NonZeroInt>(), Another.Value<NonEmptyString>()) ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>>>([ ], "CollectionOfKeyValuePairs must not be empty") ]);

		public static FirstClassCollectionOfKeyValuePairsType ApplyConstraintsTo(IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>> collectionOfKeyValuePairs)
		{
			try
			{
				return new(NonEmptyDictionary.ApplyConstraintsTo(collectionOfKeyValuePairs));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfKeyValuePairsType>(collectionOfKeyValuePairs);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfKeyValuePairsType>(developerMistake, collectionOfKeyValuePairs);
			}
		}
	}


	static readonly TestCasesAggregation<TestCase> PublicInstantiationOperationsExposingTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(PositionalRecordTypeContainingValuePropertyType),                 "{\"IntProperty\":1,\"StringProperty\":\"foo\"}"),
		new(typeof(PositionalRecordTypeContainingConstrainedValuePropertyTypes),     "{\"IntProperty\":1,\"NonEmptyGuidProperty\":\"00000000-0000-0000-0000-000000000002\"}"),
		new(typeof(PositionalRecordTypeContainingParentObjectPropertyTypes),         "{\"RecordWithAnIntAndANonEmptyGuidProperty\":{\"IntProperty\":1,\"NonEmptyGuidProperty\":\"00000000-0000-0000-0000-000000000002\"}}"),
		new(typeof(MultiplePublicConstructorsHavingParametersExposingType),          "{\"BuiltFrom\":\"two-parameters constructor called with values (1,-24)\"}"),
		new(typeof(PublicParameterlessConstructorExposingType_Aka_GetSetStyleClass), "{\"String\":\"foo\",\"Int\":1,\"NonEmptyGuid\":\"00000000-0000-0000-0000-000000000002\"}"),
		new(typeof(SinglePublicStaticFactoryMethodExposingType),                     "{\"Value\":1}"),
		new(typeof(MultiplePublicStaticFactoryMethodsExposingType),                  "{\"BuiltFrom\":\"two-parameters static factory method called with values (1,-24)\"}"),
	]);
	public class MultiplePublicConstructorsHavingParametersExposingType
	{
		public string BuiltFrom { get; }
		public MultiplePublicConstructorsHavingParametersExposingType(int i) => BuiltFrom = $"single parameter constructor called with value {i}";
		public MultiplePublicConstructorsHavingParametersExposingType(int i, int j) => BuiltFrom = $"two-parameters constructor called with values ({i},{j})";
	}
	public class PublicParameterlessConstructorExposingType_Aka_GetSetStyleClass
	{
		public string String { get; set; }
		public int Int { get; set; }
		public NonEmptyGuid NonEmptyGuid { get; set; }
	}
	public class SinglePublicStaticFactoryMethodExposingType
	{
		public int Value { get; }
		SinglePublicStaticFactoryMethodExposingType(int i) => Value = i;
		public static SinglePublicStaticFactoryMethodExposingType CreateFrom(int i) => new SinglePublicStaticFactoryMethodExposingType(i);
	}
	public class MultiplePublicStaticFactoryMethodsExposingType
	{
		public string BuiltFrom { get; }
		MultiplePublicStaticFactoryMethodsExposingType(string builtFrom) => BuiltFrom = builtFrom;
		public static MultiplePublicStaticFactoryMethodsExposingType CreateFrom(int i)        => new MultiplePublicStaticFactoryMethodsExposingType($"single parameter static factory method called with value {i}");
		public static MultiplePublicStaticFactoryMethodsExposingType CreateFrom(int i, int j) => new MultiplePublicStaticFactoryMethodsExposingType($"two-parameters static factory method called with values ({i},{j})");
	}

	static readonly TestCasesAggregation<TestCase> SingletonAccessExposingTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(PublicSingletonPropertyExposingType), "{}"),
		new(typeof(PublicSingletonFieldExposingType),    "{}"),
	]);
	public class PublicSingletonPropertyExposingType
	{
		static readonly PublicSingletonPropertyExposingType _instance = new();
		public static PublicSingletonPropertyExposingType Instance => _instance;
		PublicSingletonPropertyExposingType() { }
	}
	public class PublicSingletonFieldExposingType
	{
		public static readonly PublicSingletonFieldExposingType Instance = new();
		PublicSingletonFieldExposingType() { }
	}

	static readonly TestCasesAggregation<TestCase> InstancesAccessExposingTypes = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(PublicInstancesPropertyExposingType),      "{\"Name\":\"SecondExposed\",\"Value\":2}"),
		new(typeof(PublicInstancesReadonlyFieldExposingType), "{\"Name\":\"SecondExposed\",\"Value\":2}"),
		new(typeof(PublicInstancesFieldExposingType),         "{\"Name\":\"SecondExposed\",\"Value\":2}"),
	]);
	public class PublicInstancesPropertyExposingType
	{
		public string Name { get; }
		public int Value { get; }
		PublicInstancesPropertyExposingType(string name, int value)
		{
			Name = name;
			Value = value;
		}
		public static PublicInstancesPropertyExposingType FirstExposed => new(nameof(FirstExposed), 1);
		public static PublicInstancesPropertyExposingType SecondExposed => new(nameof(SecondExposed), 2);
	}
	public class PublicInstancesReadonlyFieldExposingType
	{
		public string Name { get; }
		public int Value { get; }
		PublicInstancesReadonlyFieldExposingType(string name, int value)
		{
			Name = name;
			Value = value;
		}
		public static readonly PublicInstancesReadonlyFieldExposingType FirstExposed = new(nameof(FirstExposed), 1);
		public static readonly PublicInstancesReadonlyFieldExposingType SecondExposed = new(nameof(SecondExposed), 2);
	}
	public class PublicInstancesFieldExposingType
	{
		public string Name { get; }
		public int Value { get; }
		PublicInstancesFieldExposingType(string name, int value)
		{
			Name = name;
			Value = value;
		}
		public static PublicInstancesFieldExposingType FirstExposed = new(nameof(FirstExposed), 1);
		public static PublicInstancesFieldExposingType SecondExposed = new(nameof(SecondExposed), 2);
	}
}
