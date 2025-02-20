using System.Collections;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases.ExampleValueGeneratorTestCases;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

public class CollectionConversionTestCases
{
	public record TestCase(Type SourceType, Type DestinationType);
	public static IEnumerable<TestCasesAggregation<TestCase>> AllTestCasesAggregatedIntoOne
	=> AllValueCollectionTypeTestCasesAggregatedIntoOne.AddVariations(
		CreateKeyValuePairTypeTestCaseFrom,
		CreateKeyValuePairTypeToDictionaryTestCaseFrom,
		CreateNonEmptyArrayTypeTestCaseFrom);

	public static IEnumerable<TestCasesAggregation<TestCase>> AllValueCollectionTypeTestCasesAggregatedIntoOne
	{
		get
		{
			yield return From_IEnumerableOfTNative______To_LinkedListOfTNative;
			yield return From_IEnumerableOfTNative______To_QueueOfTNative;
			yield return From_IEnumerableOfTNative______To_StackTNative;

			yield return From_IEnumerableOfTNative______To_ArrayOfTNative;
			yield return From_IEnumerableOfTConstrained_To_ArrayOfTConstrained;
			yield return From_IEnumerableOfTConstrained_To_ArrayOfTRoot;
			yield return From_IEnumerableOfTConstrained_To_ArrayOfTLessConstrained;
			yield return From_IEnumerableOfTEnum________To_ArrayOfStrings;
			yield return From_IEnumerableOfTEnum________To_ArrayOfInts;

			yield return From_IEnumerableOfTNative______To_ListOfTNative;
			yield return From_IEnumerableOfTConstrained_To_ListOfTConstrained;
			yield return From_IEnumerableOfTConstrained_To_ListOfTRoot;
			yield return From_IEnumerableOfTConstrained_To_ListOfTLessConstrained;
			yield return From_IEnumerableOfTEnum________To_ListOfStrings;
			yield return From_IEnumerableOfTEnum________To_ListOfInts;

			yield return From_IEnumerableOfTNative______To_IEnumerableOfTNative;
			yield return From_IEnumerableOfTConstrained_To_IEnumerableOfTConstrained;
			yield return From_IEnumerableOfTConstrained_To_IEnumerableOfTRoot;
			yield return From_IEnumerableOfTConstrained_To_IEnumerableOfTLessConstrained;
			yield return From_IEnumerableOfTEnum________To_IEnumerableOfStrings;
			yield return From_IEnumerableOfTEnum________To_IEnumerableOfInts;

			yield return From_IEnumerableOfTNative______To_IReadOnlyCollectionOfTNative;
			yield return From_IEnumerableOfTConstrained_To_IReadOnlyCollectionOfTConstrained;
			yield return From_IEnumerableOfTConstrained_To_IReadOnlyCollectionOfTRoot;
			yield return From_IEnumerableOfTConstrained_To_IReadOnlyCollectionOfTLessConstrained;
			yield return From_IEnumerableOfTEnum________To_IReadOnlyCollectionOfStrings;
			yield return From_IEnumerableOfTEnum________To_IReadOnlyCollectionOfInts;

			yield return From_IEnumerableOfTNativeKeyValuePairs______To_DictionaryOfTNatives;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_DictionaryOfTConstrained;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_DictionaryOfTRoot;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_DictionaryOfTLessConstrained;
			yield return From_IEnumerableOfTEnumKeyValuePairs________To_DictionaryOfStrings;
			yield return From_IEnumerableOfTEnumKeyValuePairs________To_DictionaryOfInts;

			yield return From_IEnumerableOfTNativeKeyValuePairs______To_IDictionaryOfTNatives;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_IDictionaryOfTConstrained;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_IDictionaryOfTRoot;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_IDictionaryOfTLessConstrained;
			yield return From_IEnumerableOfTEnumKeyValuePairs________To_IDictionaryOfStrings;
			yield return From_IEnumerableOfTEnumKeyValuePairs________To_IDictionaryOfInts;

			yield return From_IEnumerableOfTNativeKeyValuePairs______To_IReadOnlyDictionaryOfTNatives;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_IReadOnlyDictionaryOfTConstrained;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_IReadOnlyDictionaryOfTRoot;
			yield return From_IEnumerableOfTConstrainedKeyValuePairs_To_IReadOnlyDictionaryOfTLessConstrained;
			yield return From_IEnumerableOfTEnumKeyValuePairs________To_IReadOnlyDictionaryOfStrings;
			yield return From_IEnumerableOfTEnumKeyValuePairs________To_IReadOnlyDictionaryOfInts;
			yield return From_IEnumerableOfTNativeKeyValuePairs______To_DictionaryOfIEnumerablesOfTNatives;

			yield return From_IEnumerableOfTNative______To_NonGenericCollectionInterface;
			yield return From_IEnumerableOfTNativeKeyValuePairs______To_NonGenericDictionaryInterface;
		}
	}

	static TestCase CreateKeyValuePairTypeTestCaseFrom(TestCase tc)
	{
		var sourceCollectionItemType = tc.SourceType.GetCollectionItemType();
		var keyValuePairOfSourceCollectionItemType = typeof(KeyValuePair<,>).MakeGenericType(sourceCollectionItemType, sourceCollectionItemType);
		var sourceCollectionOfKeyValuePairsType = tc.SourceType switch
		{
			var t when t.IsArray => keyValuePairOfSourceCollectionItemType.MakeArrayType(),
			var t when t.IsGenericType && t.GetGenericArguments().Length == 1 => t.GetGenericTypeDefinition().MakeGenericType(keyValuePairOfSourceCollectionItemType),
			var t when t.IsGenericType && t.GetGenericArguments().Length == 2 => t.GetGenericTypeDefinition().MakeGenericType(
				typeof(KeyValuePair<,>).MakeGenericType(t.GetGenericArguments()[0], t.GetGenericArguments()[0]),
				typeof(KeyValuePair<,>).MakeGenericType(t.GetGenericArguments()[1], t.GetGenericArguments()[1])),
			_ => throw new NotImplementedException()
		};

		var destinationCollectionOfKeyValuePairsType = tc.DestinationType;
		if (tc.DestinationType.IsArray || tc.DestinationType.IsGenericType)
		{
			var destinationCollectionItemType = tc.DestinationType.GetCollectionItemType();
			var keyValuePairOfDestinationCollectionItemType = typeof(KeyValuePair<,>).MakeGenericType(destinationCollectionItemType, destinationCollectionItemType);
			destinationCollectionOfKeyValuePairsType = tc.DestinationType switch
			{
				var t when t.IsArray => keyValuePairOfDestinationCollectionItemType.MakeArrayType(),
				var t when t.IsGenericType && t.GetGenericArguments().Length == 1 => t.GetGenericTypeDefinition().MakeGenericType(keyValuePairOfDestinationCollectionItemType),
				var t when t.IsGenericType && t.GetGenericArguments().Length == 2 => t.GetGenericTypeDefinition().MakeGenericType(
					typeof(KeyValuePair<,>).MakeGenericType(t.GetGenericArguments()[0], t.GetGenericArguments()[0]),
					typeof(KeyValuePair<,>).MakeGenericType(t.GetGenericArguments()[1], t.GetGenericArguments()[1])),
				_ => throw new NotImplementedException()
			};
		}

		return new TestCase(sourceCollectionOfKeyValuePairsType, destinationCollectionOfKeyValuePairsType);
	}

	static TestCase CreateKeyValuePairTypeToDictionaryTestCaseFrom(TestCase tc)
	{
		var sourceCollectionItemType = tc.SourceType.GetCollectionItemType();
		var keyValuePairOfSourceCollectionItemType = typeof(KeyValuePair<,>).MakeGenericType(sourceCollectionItemType, sourceCollectionItemType);
		var sourceCollectionOfKeyValuePairsType = tc.SourceType switch
		{
			var t when t.IsArray => keyValuePairOfSourceCollectionItemType.MakeArrayType(),
			var t when t.IsGenericType => t.GetGenericTypeDefinition().MakeGenericType(keyValuePairOfSourceCollectionItemType),
			_ => throw new NotImplementedException()
		};

		var destinationDictionaryType = tc.DestinationType;
		if (tc.DestinationType.IsArray || tc.DestinationType.IsGenericType)
		{
			var destinationCollectionItemType = tc.DestinationType.GetCollectionItemType();
			destinationDictionaryType = typeof(Dictionary<,>).MakeGenericType(destinationCollectionItemType, destinationCollectionItemType);
		}

		return new TestCase(sourceCollectionOfKeyValuePairsType, destinationDictionaryType);
	}

	static TestCase CreateNonEmptyArrayTypeTestCaseFrom(TestCase tc)
	{
		return new TestCase(
			typeof(NonEmptyArray<>).MakeGenericType(tc.SourceType.GetCollectionItemType()),
			typeof(NonEmptyList<>).MakeGenericType(tc.DestinationType.GetCollectionItemType()));
	}

	public static IEnumerable<TestCasesAggregation<TestCase>> DestinationTypeContainingEnumType
	{
		get
		{
			yield return From_IEnumerableOfStrings______To_ArrayOfTEnum;
			yield return From_IEnumerableOfInts_________To_ArrayOfTEnum;
			yield return From_IEnumerableOfStringKeyValuePairs______To_DictionaryOfTEnum;
			yield return From_IEnumerableOfIntKeyValuePairs_________To_DictionaryOfTEnum;
		}
	}
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNative______To_ArrayOfTNative = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    int[]), typeof(int[])),
		new(typeof(               List<int> ), typeof(int[])),
		new(typeof(            HashSet<int> ), typeof(int[])),
		new(typeof(        IEnumerable<int> ), typeof(int[])),
		new(typeof(IReadOnlyCollection<int> ), typeof(int[])),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_ArrayOfTConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    PositiveInt[]), typeof(PositiveInt[])),
		new(typeof(               List<PositiveInt> ), typeof(PositiveInt[])),
		new(typeof(            HashSet<PositiveInt> ), typeof(PositiveInt[])),
		new(typeof(        IEnumerable<PositiveInt> ), typeof(PositiveInt[])),
		new(typeof(IReadOnlyCollection<PositiveInt> ), typeof(PositiveInt[])),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_ArrayOfTRoot = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    PositiveInt[]), typeof(int[])),
		new(typeof(               List<PositiveInt> ), typeof(int[])),
		new(typeof(            HashSet<PositiveInt> ), typeof(int[])),
		new(typeof(        IEnumerable<PositiveInt> ), typeof(int[])),
		new(typeof(IReadOnlyCollection<PositiveInt> ), typeof(int[])),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_ArrayOfTLessConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    StrictlyPositiveInt[]), typeof(PositiveInt[])),
		new(typeof(               List<StrictlyPositiveInt> ), typeof(PositiveInt[])),
		new(typeof(            HashSet<StrictlyPositiveInt> ), typeof(PositiveInt[])),
		new(typeof(        IEnumerable<StrictlyPositiveInt> ), typeof(PositiveInt[])),
		new(typeof(IReadOnlyCollection<StrictlyPositiveInt> ), typeof(PositiveInt[])),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnum________To_ArrayOfStrings = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    MyEnum[]), typeof(string[])),
		new(typeof(               List<MyEnum> ), typeof(string[])),
		new(typeof(            HashSet<MyEnum> ), typeof(string[])),
		new(typeof(        IEnumerable<MyEnum> ), typeof(string[])),
		new(typeof(IReadOnlyCollection<MyEnum> ), typeof(string[])),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnum________To_ArrayOfInts = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    MyEnum[]), typeof(int[])),
		new(typeof(               List<MyEnum> ), typeof(int[])),
		new(typeof(            HashSet<MyEnum> ), typeof(int[])),
		new(typeof(        IEnumerable<MyEnum> ), typeof(int[])),
		new(typeof(IReadOnlyCollection<MyEnum> ), typeof(int[])),
	]);

	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfStrings______To_ArrayOfTEnum                          = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(string[]), typeof(MyEnum[])) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfInts_________To_ArrayOfTEnum                          = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(   int[]), typeof(MyEnum[])) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfStringKeyValuePairs______To_DictionaryOfTEnum         = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(KeyValuePair<string, string>[]), typeof(Dictionary<MyEnum,MyEnum>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfIntKeyValuePairs_________To_DictionaryOfTEnum         = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(KeyValuePair<   int,    int>[]), typeof(Dictionary<MyEnum,MyEnum>)) ]);

	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNative______To_ListOfTNative                         = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(                int[]), typeof(List<        int>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_ListOfTConstrained                    = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(        PositiveInt[]), typeof(List<PositiveInt>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_ListOfTRoot                           = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(        PositiveInt[]), typeof(List<        int>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_ListOfTLessConstrained                = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(StrictlyPositiveInt[]), typeof(List<PositiveInt>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnum________To_ListOfStrings                         = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(MyEnum[]), typeof(List<string>))                   ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnum________To_ListOfInts                            = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(MyEnum[]), typeof(List<int   >))                   ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNative______To_IEnumerableOfTNative                  = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(                int[]), typeof(IEnumerable<        int>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_IEnumerableOfTConstrained             = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(        PositiveInt[]), typeof(IEnumerable<PositiveInt>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_IEnumerableOfTRoot                    = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(        PositiveInt[]), typeof(IEnumerable<        int>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_IEnumerableOfTLessConstrained         = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(StrictlyPositiveInt[]), typeof(IEnumerable<PositiveInt>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnum________To_IEnumerableOfStrings                  = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(MyEnum[]), typeof(IEnumerable<string>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnum________To_IEnumerableOfInts                     = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(MyEnum[]), typeof(IEnumerable<int>))    ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNative______To_IReadOnlyCollectionOfTNative          = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(                int[]), typeof(IReadOnlyCollection<        int>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_IReadOnlyCollectionOfTConstrained     = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(        PositiveInt[]), typeof(IReadOnlyCollection<PositiveInt>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_IReadOnlyCollectionOfTRoot            = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(        PositiveInt[]), typeof(IReadOnlyCollection<        int>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrained_To_IReadOnlyCollectionOfTLessConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(StrictlyPositiveInt[]), typeof(IReadOnlyCollection<PositiveInt>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnum________To_IReadOnlyCollectionOfStrings          = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(MyEnum[]), typeof(IReadOnlyCollection<string>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnum________To_IReadOnlyCollectionOfInts             = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(MyEnum[]), typeof(IReadOnlyCollection<int>   )) ]);

	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNative______To_LinkedListOfTNative = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(int[]), typeof(LinkedList<int>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNative______To_QueueOfTNative      = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(int[]), typeof(     Queue<int>)) ]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNative______To_StackTNative        = TestCasesAggregation<TestCase>.CreateFromTestCases([ new(typeof(int[]), typeof(     Stack<int>)) ]);

	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNativeKeyValuePairs______To_DictionaryOfTNatives = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<int, int>[]), typeof(Dictionary<int, int>)),
		new(typeof(               List<KeyValuePair<int, int>> ), typeof(Dictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<int, int>> ), typeof(Dictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<int, int>> ), typeof(Dictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<int, int>> ), typeof(Dictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_DictionaryOfTConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<PositiveInt, PositiveInt>[]), typeof(Dictionary<PositiveInt, PositiveInt>)),
		new(typeof(               List<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(Dictionary<PositiveInt, PositiveInt>)),
		new(typeof(            HashSet<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(Dictionary<PositiveInt, PositiveInt>)),
		new(typeof(        IEnumerable<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(Dictionary<PositiveInt, PositiveInt>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(Dictionary<PositiveInt, PositiveInt>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_DictionaryOfTRoot = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<PositiveInt, PositiveInt>[]), typeof(Dictionary<int, int>)),
		new(typeof(               List<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(Dictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(Dictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(Dictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(Dictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_DictionaryOfTLessConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>[]), typeof(Dictionary<PositiveInt, PositiveInt>)),
		new(typeof(               List<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(Dictionary<PositiveInt, PositiveInt>)),
		new(typeof(            HashSet<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(Dictionary<PositiveInt, PositiveInt>)),
		new(typeof(        IEnumerable<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(Dictionary<PositiveInt, PositiveInt>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(Dictionary<PositiveInt, PositiveInt>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnumKeyValuePairs________To_DictionaryOfStrings = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<MyEnum, MyEnum>[]), typeof(Dictionary<string, string>)),
		new(typeof(               List<KeyValuePair<MyEnum, MyEnum>> ), typeof(Dictionary<string, string>)),
		new(typeof(            HashSet<KeyValuePair<MyEnum, MyEnum>> ), typeof(Dictionary<string, string>)),
		new(typeof(        IEnumerable<KeyValuePair<MyEnum, MyEnum>> ), typeof(Dictionary<string, string>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<MyEnum, MyEnum>> ), typeof(Dictionary<string, string>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnumKeyValuePairs________To_DictionaryOfInts = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<MyEnum, MyEnum>[]), typeof(Dictionary<int, int>)),
		new(typeof(               List<KeyValuePair<MyEnum, MyEnum>> ), typeof(Dictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<MyEnum, MyEnum>> ), typeof(Dictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<MyEnum, MyEnum>> ), typeof(Dictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<MyEnum, MyEnum>> ), typeof(Dictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNativeKeyValuePairs______To_IDictionaryOfTNatives = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<int, int>[]), typeof(IDictionary<int, int>)),
		new(typeof(               List<KeyValuePair<int, int>> ), typeof(IDictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<int, int>> ), typeof(IDictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<int, int>> ), typeof(IDictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<int, int>> ), typeof(IDictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_IDictionaryOfTConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<PositiveInt, PositiveInt>[]), typeof(IDictionary<PositiveInt, PositiveInt>)),
		new(typeof(               List<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IDictionary<PositiveInt, PositiveInt>)),
		new(typeof(            HashSet<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IDictionary<PositiveInt, PositiveInt>)),
		new(typeof(        IEnumerable<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IDictionary<PositiveInt, PositiveInt>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IDictionary<PositiveInt, PositiveInt>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_IDictionaryOfTRoot = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<PositiveInt, PositiveInt>[]), typeof(IDictionary<int, int>)),
		new(typeof(               List<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IDictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IDictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IDictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IDictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_IDictionaryOfTLessConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>[]), typeof(IDictionary<PositiveInt, PositiveInt>)),
		new(typeof(               List<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(IDictionary<PositiveInt, PositiveInt>)),
		new(typeof(            HashSet<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(IDictionary<PositiveInt, PositiveInt>)),
		new(typeof(        IEnumerable<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(IDictionary<PositiveInt, PositiveInt>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(IDictionary<PositiveInt, PositiveInt>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnumKeyValuePairs________To_IDictionaryOfStrings = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<MyEnum, MyEnum>[]), typeof(IDictionary<string, string>)),
		new(typeof(               List<KeyValuePair<MyEnum, MyEnum>> ), typeof(IDictionary<string, string>)),
		new(typeof(            HashSet<KeyValuePair<MyEnum, MyEnum>> ), typeof(IDictionary<string, string>)),
		new(typeof(        IEnumerable<KeyValuePair<MyEnum, MyEnum>> ), typeof(IDictionary<string, string>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<MyEnum, MyEnum>> ), typeof(IDictionary<string, string>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnumKeyValuePairs________To_IDictionaryOfInts = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<MyEnum, MyEnum>[]), typeof(IDictionary<int, int>)),
		new(typeof(               List<KeyValuePair<MyEnum, MyEnum>> ), typeof(IDictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<MyEnum, MyEnum>> ), typeof(IDictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<MyEnum, MyEnum>> ), typeof(IDictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<MyEnum, MyEnum>> ), typeof(IDictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNativeKeyValuePairs______To_IReadOnlyDictionaryOfTNatives = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<int, int>[]), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(               List<KeyValuePair<int, int>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<int, int>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<int, int>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<int, int>> ), typeof(IReadOnlyDictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_IReadOnlyDictionaryOfTConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<PositiveInt, PositiveInt>[]), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
		new(typeof(               List<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
		new(typeof(            HashSet<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
		new(typeof(        IEnumerable<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_IReadOnlyDictionaryOfTRoot = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<PositiveInt, PositiveInt>[]), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(               List<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<PositiveInt, PositiveInt>> ), typeof(IReadOnlyDictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTConstrainedKeyValuePairs_To_IReadOnlyDictionaryOfTLessConstrained = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>[]), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
		new(typeof(               List<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
		new(typeof(            HashSet<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
		new(typeof(        IEnumerable<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<StrictlyPositiveInt, StrictlyPositiveInt>> ), typeof(IReadOnlyDictionary<PositiveInt, PositiveInt>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnumKeyValuePairs________To_IReadOnlyDictionaryOfStrings = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<MyEnum, MyEnum>[]), typeof(IReadOnlyDictionary<string, string>)),
		new(typeof(               List<KeyValuePair<MyEnum, MyEnum>> ), typeof(IReadOnlyDictionary<string, string>)),
		new(typeof(            HashSet<KeyValuePair<MyEnum, MyEnum>> ), typeof(IReadOnlyDictionary<string, string>)),
		new(typeof(        IEnumerable<KeyValuePair<MyEnum, MyEnum>> ), typeof(IReadOnlyDictionary<string, string>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<MyEnum, MyEnum>> ), typeof(IReadOnlyDictionary<string, string>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTEnumKeyValuePairs________To_IReadOnlyDictionaryOfInts = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<MyEnum, MyEnum>[]), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(               List<KeyValuePair<MyEnum, MyEnum>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(            HashSet<KeyValuePair<MyEnum, MyEnum>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(        IEnumerable<KeyValuePair<MyEnum, MyEnum>> ), typeof(IReadOnlyDictionary<int, int>)),
		new(typeof(IReadOnlyCollection<KeyValuePair<MyEnum, MyEnum>> ), typeof(IReadOnlyDictionary<int, int>)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNativeKeyValuePairs______To_DictionaryOfIEnumerablesOfTNatives = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<                    int[],                     int[]>[]), typeof(Dictionary<IEnumerable<int>, IReadOnlyCollection<int>>)),
		new(typeof(               List<KeyValuePair<               List<int> ,                List<int  >>>), typeof(Dictionary<      IList<int>,       IReadOnlyList<int>>)),
		new(typeof(            HashSet<KeyValuePair<            HashSet<int> ,             HashSet<int  >>>), typeof(Dictionary<       ISet<int>,        IReadOnlySet<int>>)),
		new(typeof(        IEnumerable<KeyValuePair<        IEnumerable<int> ,         IEnumerable<int  >>>), typeof(Dictionary<ICollection<int>,         IEnumerable     >)),
		new(typeof(IReadOnlyCollection<KeyValuePair<IReadOnlyCollection<int> , IReadOnlyCollection<int  >>>), typeof(Dictionary<       IList    ,         ICollection     >)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNative______To_NonGenericCollectionInterface = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                            int[]), typeof(IList)),
		new(typeof(               List<PositiveInt >), typeof(ICollection)),
		new(typeof(                    HashSet<int >), typeof(IEnumerable)),
		new(typeof(        IEnumerable<PositiveInt >), typeof(IEnumerable)),
		new(typeof(IReadOnlyCollection<        int >), typeof(IEnumerable)),
	]);
	static readonly TestCasesAggregation<TestCase> From_IEnumerableOfTNativeKeyValuePairs______To_NonGenericDictionaryInterface = TestCasesAggregation<TestCase>.CreateFromTestCases(
	[
		new(typeof(                    KeyValuePair<        int,         PositiveInt>[]), typeof(IDictionary)),
		new(typeof(               List<KeyValuePair<PositiveInt,                 int>> ), typeof(IDictionary)),
		new(typeof(            HashSet<KeyValuePair<PositiveInt, StrictlyPositiveInt>> ), typeof(IDictionary)),
		new(typeof(        IEnumerable<KeyValuePair<        int,                 int>> ), typeof(IDictionary)),
		new(typeof(IReadOnlyCollection<KeyValuePair<        int,                 int>> ), typeof(IDictionary)),
	]);
}
