using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class SystemGenericCollectionExampleInstantiationBuilder : RandomizedExampleInstantiationBuilder
{
	public static readonly SystemGenericCollectionExampleInstantiationBuilder Instance = new();
	static Type GetNonEmptyCollectionTypeMatching(Type collectionType)
	=> collectionType switch
	{
		var t when t.IsArray => typeof(NonEmptyArray<>).MakeGenericType(t.GetCollectionItemType()),
		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
		{
			var gt when gt == typeof(HashSet<>)     => typeof(NonEmptyHashSet<>).MakeGenericType(t.GetCollectionItemType()),
			var gt when gt == typeof(Dictionary<,>) => typeof(NonEmptyDictionary<,>).MakeGenericType(t.GetGenericArguments()),
			var gt when gt.IsInterface => gt switch
			{
				var gi when gi == typeof(IDictionary<,>)         => typeof(NonEmptyDictionary<,>).MakeGenericType(t.GetGenericArguments()),
				var gi when gi == typeof(IReadOnlyDictionary<,>) => typeof(NonEmptyDictionary<,>).MakeGenericType(t.GetGenericArguments()),
				var gi when gi == typeof(ICollection<>)          => typeof(NonEmptyArray<>).MakeGenericType(t.GetCollectionItemType()),
				var gi when gi == typeof(IEnumerable<>)          => typeof(NonEmptyArray<>).MakeGenericType(t.GetCollectionItemType()),
				var gi when gi == typeof(IList<>)                => typeof(NonEmptyArray<>).MakeGenericType(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlyCollection<>)  => typeof(NonEmptyArray<>).MakeGenericType(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlyList<>)        => typeof(NonEmptyArray<>).MakeGenericType(t.GetCollectionItemType()),
				var gi when gi == typeof(IReadOnlySet<>)         => typeof(NonEmptyHashSet<>).MakeGenericType(t.GetCollectionItemType()),
				var gi when gi == typeof(ISet<>)                 => typeof(NonEmptyHashSet<>).MakeGenericType(t.GetCollectionItemType()),
				_ => throw new NotImplementedException()
			},
			var gt when gt == typeof(LinkedList<>)        => typeof(NonEmptyLinkedList<>).MakeGenericType(t.GetCollectionItemType()),
			var gt when gt == typeof(List<>)              => typeof(NonEmptyList<>).MakeGenericType(t.GetCollectionItemType()),
			var gt when gt == typeof(Queue<>)             => typeof(NonEmptyQueue<>).MakeGenericType(t.GetCollectionItemType()),
			var gt when gt == typeof(SortedSet<>)         => typeof(NonEmptySortedSet<>).MakeGenericType(t.GetCollectionItemType()),
			var gt when gt == typeof(Stack<>)             => typeof(NonEmptyStack<>).MakeGenericType(t.GetCollectionItemType()),
			var gt when gt == typeof(SortedDictionary<,>) => typeof(NonEmptySortedDictionary<,>).MakeGenericType(t.GetGenericArguments()),
			var gt when gt == typeof(SortedList<,>)       => typeof(NonEmptySortedList<,>).MakeGenericType(t.GetGenericArguments()),
			_ => throw new NotImplementedException()
		},
		_ => throw new NotImplementedException()
	};

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var salt = Expression.Variable(RandomSalt.Type, "salt");
		var collection = Expression.Variable(type, "collection");
		return Expression.Block(
			[salt, collection],
			Expression.Assign(salt, RandomSalt.SelectBetweenOneAndMeanRecurrenceIntervalForEmptyCollectionGeneration),
			Expression.IfThenElse(
				Expression.Equal(salt, RandomSalt.ConstantLowestPossibleValue),
				Expression.Assign(
					collection,
					BuildEmptyCollectionInstantiationExpressionFor(type, generativeSalts)),
				Expression.Assign(
					collection,
					CollectionConversionBuilder.Build(
						RandomizedExampleInstantiationBuilder.BuildFor(
							GetNonEmptyCollectionTypeMatching(type),
							currentSalt,
							generativeSalts),
						type))),
			collection);
	}

	static BlockExpression BuildEmptyCollectionInstantiationExpressionFor(Type type, Expression generativeSalts)
	{
		var emptyCollection = Expression.Variable(type, "emptyCollection");
		return Expression.Block(
			[emptyCollection],
			GenerativeSalts.Enqueue(generativeSalts, GenerativeSalts.MarkerForEmptySystemGenericCollections),
			Expression.Assign(emptyCollection, Expression.Call(ReturnEmptyInstanceInstantiationMethod(type))),
			emptyCollection);
	}

	static Dictionary<TKey, TValue> CreateEmptyDictionary<TKey, TValue>() => [];
	static HashSet<T> CreateEmptyHashSet<T>() => [];
	static T[] CreateEmptyArray<T>() => [];
	static LinkedList<T> CreateEmptyLinkedList<T>() => [];
	static List<T> CreateEmptyList<T>() => [];
	static Queue<T> CreateEmptyQueue<T>() => [];
	static SortedSet<T> CreateEmptySortedSet<T>() => [];
	static Stack<T> CreateEmptyStack<T>() => [];
	static SortedDictionary<T,U> CreateEmptySortedDictionary<T,U>() => [];
	static SortedList<T,U> CreateEmptySortedList<T,U>() => [];

	static readonly Type ThisType = typeof(SystemGenericCollectionExampleInstantiationBuilder);
	static readonly MethodInfo OpenGenericDictionaryInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptyDictionary), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericHashSetInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptyHashSet), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericArrayInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptyArray), BindingFlags.Static | BindingFlags.NonPublic);

	static readonly MethodInfo OpenGenericLinkedListInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptyLinkedList), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericListInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptyList), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericQueueInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptyQueue), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedSetInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptySortedSet), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericStackInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptyStack), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedDictionaryInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptySortedDictionary), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedSortedListInstantiationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstantiationBuilder.CreateEmptySortedList), BindingFlags.Static | BindingFlags.NonPublic);

	static MethodInfo ReturnEmptyInstanceInstantiationMethod(Type type)
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
