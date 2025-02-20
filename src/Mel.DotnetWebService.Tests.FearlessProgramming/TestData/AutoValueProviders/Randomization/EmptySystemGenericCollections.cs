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
	static readonly MethodInfo OpenGenericDictionaryInstanciationMethod       = ThisType.GetMethod(nameof(CreateEmptyDictionary),       BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericHashSetInstanciationMethod          = ThisType.GetMethod(nameof(CreateEmptyHashSet),          BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericArrayInstanciationMethod            = ThisType.GetMethod(nameof(CreateEmptyArray),            BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericLinkedListInstanciationMethod       = ThisType.GetMethod(nameof(CreateEmptyLinkedList),       BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericListInstanciationMethod             = ThisType.GetMethod(nameof(CreateEmptyList),             BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericQueueInstanciationMethod            = ThisType.GetMethod(nameof(CreateEmptyQueue),            BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedSetInstanciationMethod        = ThisType.GetMethod(nameof(CreateEmptySortedSet),        BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericStackInstanciationMethod            = ThisType.GetMethod(nameof(CreateEmptyStack),            BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedDictionaryInstanciationMethod = ThisType.GetMethod(nameof(CreateEmptySortedDictionary), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedSortedListInstanciationMethod = ThisType.GetMethod(nameof(CreateEmptySortedList),       BindingFlags.Static | BindingFlags.NonPublic);

	public static MethodInfo GetEmptyCollectionInstanciationMethod(Type type)
	=> type switch
	{
		var t when t.IsArray => typeof(Array).GetMethod(nameof(Array.Empty)).MakeGenericMethod(t.GetCollectionItemType()),
		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
		{
			var gt when gt == typeof(HashSet<>)     => OpenGenericHashSetInstanciationMethod.MakeGenericMethod(t.GetGenericArguments()),
			var gt when gt == typeof(Dictionary<,>) => OpenGenericDictionaryInstanciationMethod.MakeGenericMethod(t.GetGenericArguments()),
			var gt when gt.IsInterface => gt switch
			{
				var gi when gi == typeof(IDictionary<,>)         => OpenGenericDictionaryInstanciationMethod.MakeGenericMethod(t.GetGenericArguments()),
				var gi when gi == typeof(IReadOnlyDictionary<,>) => OpenGenericDictionaryInstanciationMethod.MakeGenericMethod(t.GetGenericArguments()),
				var gi when gi == typeof(ICollection<>)          => OpenGenericArrayInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IEnumerable<>)          => OpenGenericArrayInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IList<>)                => OpenGenericArrayInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlyCollection<>)  => OpenGenericArrayInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlyList<>)        => OpenGenericArrayInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlySet<>)         => OpenGenericHashSetInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				var gi when gi == typeof(ISet<>)                 => OpenGenericHashSetInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
				_ => throw new NotImplementedException()
			},
			var gt when gt == typeof(LinkedList<>)        => OpenGenericLinkedListInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(List<>)              => OpenGenericListInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(Queue<>)             => OpenGenericQueueInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(SortedSet<>)         => OpenGenericSortedSetInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(Stack<>)             => OpenGenericStackInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(SortedDictionary<,>) => OpenGenericSortedDictionaryInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			var gt when gt == typeof(SortedList<,>)       => OpenGenericSortedSortedListInstanciationMethod.MakeGenericMethod(t.GetCollectionItemType()),
			_ => throw new NotImplementedException()
		},
		_ => throw new NotImplementedException()
	};
}
