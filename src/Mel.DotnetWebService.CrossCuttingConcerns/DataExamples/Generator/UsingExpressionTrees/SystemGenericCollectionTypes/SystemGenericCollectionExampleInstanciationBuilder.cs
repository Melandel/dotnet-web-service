using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

abstract class SystemGenericCollectionExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	abstract protected bool AllowsDuplicateElements { get; }
	protected Expression BuildFirstItemInstanciationExpression(Type collectionItemType, Expression currentSalt)
	=> ExampleInstanciationBuilder.BuildFor(collectionItemType, currentSalt);

	protected Expression BuildSecondItemInstanciationExpression(Type collectionItemType, Expression currentSalt)
	=> ExampleInstanciationBuilder.BuildFor(collectionItemType, AllowsDuplicateElements ? currentSalt : Expression.Add(currentSalt, Expression.Constant(1)));

	public static SystemGenericCollectionExampleInstanciationBuilder InstanceFor(Type type)
	=> type switch
	{
		var t when t.IsArray => ArrayExampleInstanciationBuilder.Instance,
		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
		{
			var gt when gt == typeof(HashSet<>) => HashSetExampleInstanciationBuilder.Instance,
			var gt when gt == typeof(Dictionary<,>) => DictionaryExampleInstanciationBuilder.Instance,
			var gt when gt.IsInterface => gt switch
			{
				var gi when gi == typeof(IDictionary<,>)         => InstanceFor(typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments())),
				var gi when gi == typeof(IReadOnlyDictionary<,>) => InstanceFor(typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments())),
				var gi when gi == typeof(ICollection<>)          => InstanceFor(type.GetCollectionItemType().MakeArrayType()),
				var gi when gi == typeof(IEnumerable<>)          => InstanceFor(type.GetCollectionItemType().MakeArrayType()),
				var gi when gi == typeof(IList<>)                => InstanceFor(type.GetCollectionItemType().MakeArrayType()),
				var gi when gi == typeof(IReadOnlyCollection<>)  => InstanceFor(type.GetCollectionItemType().MakeArrayType()),
				var gi when gi == typeof(IReadOnlyList<>)        => InstanceFor(type.GetCollectionItemType().MakeArrayType()),
				var gi when gi == typeof(IReadOnlySet<>)         => InstanceFor(typeof(HashSet<>).MakeGenericType(type.GetCollectionItemType())),
				var gi when gi == typeof(ISet<>)                 => InstanceFor(typeof(HashSet<>).MakeGenericType(type.GetCollectionItemType())),
				_ => throw new NotImplementedException()
			},
			var gt when gt == typeof(LinkedList<>)        => LinkedListExampleInstanciationBuilder.Instance,
			var gt when gt == typeof(List<>)              => ListExampleInstanciationBuilder.Instance,
			var gt when gt == typeof(Queue<>)             => QueueExampleInstanciationBuilder.Instance,
			var gt when gt == typeof(SortedSet<>)         => SortedSetExampleInstanciationBuilder.Instance,
			var gt when gt == typeof(Stack<>)             => StackExampleInstanciationBuilder.Instance,
			var gt when gt == typeof(SortedDictionary<,>) => SortedDictionaryExampleInstanciationBuilder.Instance,
			var gt when gt == typeof(SortedList<,>)       => SortedListExampleInstanciationBuilder.Instance,
			_ => throw new NotImplementedException()
		},
		_ => throw new NotImplementedException()
	};
}
