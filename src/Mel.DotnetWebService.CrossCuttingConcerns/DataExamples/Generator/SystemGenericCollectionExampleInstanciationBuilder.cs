using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class SystemGenericCollectionExampleInstanciationBuilder : ExampleInstanciationBuilder
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

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	=> CollectionConversionCallExpressionBuilder.Build(
		ExampleInstanciationBuilder.BuildFor(GetNonEmptyCollectionTypeMatching(type), currentSalt),
		type);
}
