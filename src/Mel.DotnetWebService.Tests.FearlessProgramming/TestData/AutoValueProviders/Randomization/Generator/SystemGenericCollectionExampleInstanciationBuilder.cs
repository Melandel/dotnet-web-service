using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class SystemGenericCollectionExampleInstanciationBuilder : RandomizedExampleInstanciationBuilder
{
	public static readonly SystemGenericCollectionExampleInstanciationBuilder Instance = new();
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

	static readonly Expression ConstantOne = Expression.Constant(1);
	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var salt = Expression.Variable(RandomSalt.Type, "salt");
		var collection = Expression.Variable(type, "collection");
		return Expression.Block(
			[salt, collection],
			Expression.Assign(salt, RandomSalt.SelectBetweenOneAndTen),
			Expression.IfThenElse(
				Expression.Equal(salt, ConstantOne),
				Expression.Assign(
					collection,
					BuildEmptyCollectionInstanciationExpressionFor(type, generativeSalts)),
				Expression.Assign(
					collection,
					CollectionConversionCallExpressionBuilder.Build(
						RandomizedExampleInstanciationBuilder.BuildFor(
							GetNonEmptyCollectionTypeMatching(type),
							currentSalt,
							generativeSalts),
						type))),
			collection);
	}

	static BlockExpression BuildEmptyCollectionInstanciationExpressionFor(Type type, Expression generativeSalts)
	{
		var emptyCollection = Expression.Variable(type, "emptyCollection");
		return Expression.Block(
			[emptyCollection],
			GenerativeSalts.Enqueue(generativeSalts, GenerativeSalts.MarkerForEmptySystemGenericCollections),
			Expression.Assign(emptyCollection, Expression.Call(ReturnEmptyInstanceInstanciationMethod(type))),
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

	static readonly Type ThisType = typeof(SystemGenericCollectionExampleInstanciationBuilder);
	static readonly MethodInfo OpenGenericDictionaryInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptyDictionary), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericHashSetInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptyHashSet), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericArrayInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptyArray), BindingFlags.Static | BindingFlags.NonPublic);

	static readonly MethodInfo OpenGenericLinkedListInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptyLinkedList), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericListInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptyList), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericQueueInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptyQueue), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedSetInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptySortedSet), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericStackInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptyStack), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedDictionaryInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptySortedDictionary), BindingFlags.Static | BindingFlags.NonPublic);
	static readonly MethodInfo OpenGenericSortedSortedListInstanciationMethod = ThisType.GetMethod(nameof(SystemGenericCollectionExampleInstanciationBuilder.CreateEmptySortedList), BindingFlags.Static | BindingFlags.NonPublic);

	static MethodInfo ReturnEmptyInstanceInstanciationMethod(Type type)
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
