using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization;

static class EmptySystemGenericCollections
{
	static Dictionary<TKey, TValue> CreateEmptyDictionary<TKey, TValue>() => [];
	static HashSet<T>               CreateEmptyHashSet<T>()               => [];
	static T[]                      CreateEmptyArray<T>()                 => [];
	static LinkedList<T>            CreateEmptyLinkedList<T>()            => [];
	static List<T>                  CreateEmptyList<T>()                  => [];
	static Queue<T>                 CreateEmptyQueue<T>()                 => [];
	static SortedSet<T>             CreateEmptySortedSet<T>()             => [];
	static Stack<T>                 CreateEmptyStack<T>()                 => [];
	static SortedDictionary<T,U>    CreateEmptySortedDictionary<T,U>()    => [];
	static SortedList<T,U>          CreateEmptySortedList<T,U>()          => [];

	static readonly Type ThisType = typeof(EmptySystemGenericCollections);
	static readonly MethodInfo OpenGenericDictionaryInstantiationMethod       = ThisType.GetMethod(nameof(CreateEmptyDictionary),       BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericHashSetInstantiationMethod          = ThisType.GetMethod(nameof(CreateEmptyHashSet),          BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericArrayInstantiationMethod            = ThisType.GetMethod(nameof(CreateEmptyArray),            BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericLinkedListInstantiationMethod       = ThisType.GetMethod(nameof(CreateEmptyLinkedList),       BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericListInstantiationMethod             = ThisType.GetMethod(nameof(CreateEmptyList),             BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericQueueInstantiationMethod            = ThisType.GetMethod(nameof(CreateEmptyQueue),            BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedSetInstantiationMethod        = ThisType.GetMethod(nameof(CreateEmptySortedSet),        BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericStackInstantiationMethod            = ThisType.GetMethod(nameof(CreateEmptyStack),            BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedDictionaryInstantiationMethod = ThisType.GetMethod(nameof(CreateEmptySortedDictionary), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedSortedListInstantiationMethod = ThisType.GetMethod(nameof(CreateEmptySortedList),       BindingFlags.Static | BindingFlags.NonPublic);

	public static MethodInfo GetEmptyCollectionInstantiationMethod(Type type)
	=> type switch
	{
		var t when t.IsArray => typeof(Array).GetMethod(nameof(Array.Empty)).MakeGenericMethod(t.GetCollectionItemType()),
		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
		{
			var gt when gt == typeof(HashSet<>)     => OpenGenericHashSetInstantiationMethod.MakeGenericMethod(t.GetGenericArguments()),
			var gt when gt == typeof(Dictionary<,>) => OpenGenericDictionaryInstantiationMethod.MakeGenericMethod(t.GetGenericArguments()),
			var gt when gt.IsInterface => gt switch
			{
				var gi when gi == typeof(IDictionary<,>)         => OpenGenericDictionaryInstantiationMethod.MakeGenericMethod(t.GetGenericArguments()),
				var gi when gi == typeof(IReadOnlyDictionary<,>) => OpenGenericDictionaryInstantiationMethod.MakeGenericMethod(t.GetGenericArguments()),
				var gi when gi == typeof(ICollection<>)          => OpenGenericArrayInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IEnumerable<>)          => OpenGenericArrayInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IList<>)                => OpenGenericArrayInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlyCollection<>)  => OpenGenericArrayInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlyList<>)        => OpenGenericArrayInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlySet<>)         => OpenGenericHashSetInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(ISet<>)                 => OpenGenericHashSetInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				_ => throw new NotImplementedException()
			},
			var gt when gt == typeof(LinkedList<>)        => OpenGenericLinkedListInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(List<>)              => OpenGenericListInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(Queue<>)             => OpenGenericQueueInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(SortedSet<>)         => OpenGenericSortedSetInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(Stack<>)             => OpenGenericStackInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(SortedDictionary<,>) => OpenGenericSortedDictionaryInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(SortedList<,>)       => OpenGenericSortedSortedListInstantiationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			_ => throw new NotImplementedException()
		},
		_ => throw new NotImplementedException()
	};
}
