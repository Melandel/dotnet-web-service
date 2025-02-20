using System.Collections;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

// ✘ big todo!
// IF GENERIC
// 	Convert to the right GenericType
// 		Find the conversion path between the two collection types
// 	Convert the item type if necessary
// 		Find the conversion path between the two item types
abstract class CollectionConverter
{
	public static TDest Convert<TSource,TDest>(TSource collection)
		where TSource: notnull
	=> (TDest) Convert(collection, typeof(TDest));

	public static object Convert(object collection, Type destinationType)
	{
		if (collection.GetType() == destinationType)
		{
			return collection;
		}

		//return CollectionToDictionaryConverter.Instance.ConvertCollection(collection, to);
		CollectionConverter converter = destinationType switch
		{
			var t when t.IsInterface => t switch
			{
				var itf when itf == typeof(IDictionary) => CollectionToGenericDictionaryConverter.Instance,
				_ => CollectionToGenericListConverter.Instance, // Covers IList, ICollection, and most IEnumerable<T>
			},
			_ => CollectionToClassWithMatchingSingleInstanciatorParameterConverter.Instance
	//		var t when t.IsArray => CollectionToArrayConverter.Instance,
	//		var t when t.ImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out _) => CollectionToConstrainedKeyValuePairsConverter.Instance,
	//		var t when t.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out _) => CollectionToConstrainedCollectionConverter.Instance,
	//		var t when t.IsInterface => t switch
	//		{
	//			var itf when itf.IsGenericType => itf.GetGenericTypeDefinition() switch
	//			{
	//				var igd when igd == typeof(IList<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(IEnumerable<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(ICollection<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(IReadOnlyCollection<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(ISet<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(IReadOnlySet<>) => CollectionToListConverter.Instance,
	//				_ => NotImplementedCollectionConverter.Instance
	//			},
	//			var itf when itf == typeof(System.Collections.IEnumerable) => NotImplementedCollectionConverter.Instance,
	//			var itf when itf == typeof(System.Collections.ICollection) => NotImplementedCollectionConverter.Instance,
	//			var itf when itf == typeof(System.Collections.IDictionary) => NotImplementedCollectionConverter.Instance,
	//			_ => NotImplementedCollectionConverter.Instance
	//		},
	//		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
	//		{
	//			var gt when gt == typeof(HashSet<>) => CollectionToHashSetConverter.Instance,
	//			var gt when gt == typeof(LinkedList<>) => CollectionToLinkedListConverter.Instance,
	//			var gt when gt == typeof(List<>) => CollectionToListConverter.Instance,
	//			var gt when gt == typeof(Queue<>) => CollectionToQueueConverter.Instance,
	//			var gt when gt == typeof(SortedSet<>) => CollectionToSortedSetConverter.Instance,
	//			var gt when gt == typeof(Stack<>) => CollectionToStackConverter.Instance,
	//			var gt when gt == typeof(Dictionary<,>) => CollectionToDictionaryConverter.Instance,
	//			var gt when gt == typeof(SortedDictionary<,>) => CollectionToSortedDictionaryConverter.Instance,
	//			var gt when gt == typeof(SortedList<,>) => CollectionToSortedListConverter.Instance,
	//			_ => NotImplementedCollectionConverter.Instance
	//		},
	//		_ => CollectionToClassWithMatchingSingleInstanciatorParameterConverter.Instance
		};
		return converter.ConvertCollection(collection, destinationType);
	}

	public abstract object ConvertCollection(object collection, Type to);
}

class CollectionToClassWithMatchingSingleInstanciatorParameterConverter : CollectionConverter
{
	public static readonly CollectionToClassWithMatchingSingleInstanciatorParameterConverter Instance = new();

	public override object ConvertCollection(object collection, Type destinationType)
	{
		var listType = typeof(List<>).MakeGenericType(collection.GetType().GetCollectionItemType());
		object list = CollectionConverter.Convert(collection, listType);
		var classWithMatchingSingleInstanciatorParameter = destinationType.CreateInstanceUsingConstructorOrFactoryMethod(list, BindingFlags.Public);
		return classWithMatchingSingleInstanciatorParameter;
	}
}
// class CollectionToConstrainedCollectionConverter : CollectionConverter
// {
// 	public static readonly CollectionToConstrainedCollectionConverter Instance = new();
// }
//
// class CollectionToConstrainedKeyValuePairsConverter : CollectionConverter
// {
// 	public static readonly CollectionToConstrainedKeyValuePairsConverter Instance = new();
// }

