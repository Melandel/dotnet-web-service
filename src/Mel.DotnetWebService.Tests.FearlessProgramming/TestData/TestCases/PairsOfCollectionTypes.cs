using System.Collections;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

//class PairsOfCollectionTypes : IEnumerable
//{
//	public IEnumerator GetEnumerator()
//	{
//		// TODO : make a pair
//		foreach (var type in ExampleValueGeneratorTestCases.AllTypes)
//		{
//				var itemType = ((object[])type)[0] as Type;
//			foreach (var nativeCollectionType in NativeCollectionTypes)
//			{
//				yield return nativeCollectionType.MakeGenericType(itemType);
//			}
//			foreach (var nativeCollectionOfKvpType in NativeCollectionOfKeyValuePairsTypes)
//			{
//				yield return nativeCollectionOfKvpType.MakeGenericType(typeof(int), itemType);
//				yield return nativeCollectionOfKvpType.MakeGenericType(typeof(NonEmptyGuid), itemType);
//			}
//		}
//	}
//
//	static readonly Type[] NativeCollectionTypes =
//	[
//		typeof(Array),
//		typeof(HashSet<>),
//		typeof(LinkedList<>),
//		typeof(List<>),
//		typeof(Queue<>),
//		typeof(SortedSet<>),
//		typeof(Stack<>),
//	];
//
//	static readonly Type[] NativeCollectionOfKeyValuePairsTypes =
//	[
//		typeof(Dictionary<,>),
//		typeof(SortedDictionary<,>),
//		typeof(SortedList<,>),
//	];
//}
