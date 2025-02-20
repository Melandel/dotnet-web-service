using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

// class CollectionToArrayConverter : CollectionConverter
// {
// 	public static readonly CollectionToArrayConverter Instance = new();
// }
//
class CollectionToGenericListConverter : CollectionConverter
{
	public static readonly CollectionToGenericListConverter Instance = new();

	public override object ConvertCollection(object collection, Type destinationType)
	{
		// 		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		var collectionType = collection.GetType();
		var shareSameItemType = collectionType.GetCollectionItemType() == destinationType.GetCollectionItemType();
		if (shareSameItemType)
		{
			var sharedItemType = collectionType.GetCollectionItemType();
			if (collection is ICollection iCollection)
			{
				return GenericListBuilder
					.For(sharedItemType, iCollection.Count)
					.AddRange(iCollection)
					.BuildAsIList();
			}
			if (collection is IEnumerable iEnumerable)
			{
				return GenericListBuilder
					.For(sharedItemType)
					.AddRange(iEnumerable)
					.BuildAsIList();
			}
			throw new InvalidOperationException();
		}

		var shareSameEnumerableType =
			collectionType.IsGenericType && destinationType.IsGenericType
			&& collectionType.GetGenericTypeDefinition() == destinationType.GetGenericTypeDefinition();
		if (shareSameEnumerableType)
		{
			var collectionItemType = collectionType.GetCollectionItemType();
			var destinationItemType = destinationType.GetCollectionItemType();
			// find conversion path between the two item types
			// 1. Implicit conversion or chain of implicit conversions
			// 2. ConstrainedType => FactoryMethod (value vs collection) or chain of factoryMethods
			Func converter = GetconverterBetween(collectionItemType, destinationItemType);
			var listOfDestinationItemTypesBuilder = collection is ICollection iCollection
				? GenericListBuilder.For(destinationItemType, iCollection.Count)
				: GenericListBuilder.For(destinationItemType);
			IEnumerable collectionAsEnumerable = (IEnumerable)collection;
			foreach (var item in collectionAsEnumerable)
			{
				var itemAsDestinationItemType = converter.Invoke(item);
				listOfDestinationItemTypesBuilder.Add(itemAsDestinationItemType);
			}
			return listOfDestinationItemTypesBuilder.BuildAsIList();
		}

		throw new NotImplementedException();
	}

	Func GetconverterBetween(Type collectionItemType, Type destinationItemType)
	{
		return null!;
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


