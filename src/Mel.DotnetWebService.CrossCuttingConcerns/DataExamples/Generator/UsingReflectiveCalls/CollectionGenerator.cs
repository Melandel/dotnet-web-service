using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingReflectiveCalls;

class CollectionGenerator : ExampleValueGeneratorUsingReflectiveCalls
{
	public static readonly CollectionGenerator Instance = new();
	CollectionGenerator()
	{
	}

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var itemType = type.GetCollectionItemType();
		var listBuilder = GenericListBuilder.For(itemType);
		var list = GenericListBuilder.For(itemType)
			.Add(ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(itemType, salt))
			.Add(ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(itemType, salt + GetSaltOffsetForSecondExample(type)))
			.BuildAsIList();

		return CollectionConverter.Convert(list, type);
	}

	static int GetSaltOffsetForSecondExample(Type type)
	=>  type switch
	{
		// 0: Populate example collection with duplicate elements (same salt)
		// 1: Populate example collection with different elements (different salt)
		var t when t.IsArray => 0,
		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
		{
			var gt when gt == typeof(HashSet<>) => 1,
			var gt when gt.IsInterface => gt switch
			{
				var gi when gi == typeof(ICollection<>)          => GetSaltOffsetForSecondExample(typeof(        object[])),
				var gi when gi == typeof(IDictionary<,>)         => GetSaltOffsetForSecondExample(typeof(HashSet<object> )),
				var gi when gi == typeof(IEnumerable<>)          => GetSaltOffsetForSecondExample(typeof(        object[])),
				var gi when gi == typeof(IList<>)                => GetSaltOffsetForSecondExample(typeof(        object[])),
				var gi when gi == typeof(IReadOnlyCollection<>)  => GetSaltOffsetForSecondExample(typeof(        object[])),
				var gi when gi == typeof(IReadOnlyDictionary<,>) => GetSaltOffsetForSecondExample(typeof(HashSet<object> )),
				var gi when gi == typeof(IReadOnlyList<>)        => GetSaltOffsetForSecondExample(typeof(        object[])),
				var gi when gi == typeof(IReadOnlySet<>)         => GetSaltOffsetForSecondExample(typeof(HashSet<object> )),
				var gi when gi == typeof(ISet<>)                 => GetSaltOffsetForSecondExample(typeof(HashSet<object> )),
				_ => 1
			},
			var gt when gt == typeof(LinkedList<>) => 0,
			var gt when gt == typeof(List<>) => 0,
			var gt when gt == typeof(Queue<>) => 0,
			var gt when gt == typeof(SortedSet<>) => 1,
			var gt when gt == typeof(Stack<>) => 0,
			var gt when gt == typeof(Dictionary<,>) => 1,
			var gt when gt == typeof(SortedDictionary<,>) => 1,
			var gt when gt == typeof(SortedList<,>) => 1,
			_ => 1
		},
		_ => 1
	};
}
