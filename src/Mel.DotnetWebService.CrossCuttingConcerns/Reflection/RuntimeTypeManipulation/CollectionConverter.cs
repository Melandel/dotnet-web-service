using System.Collections;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

static class CollectionConverter
{
	static readonly Dictionary<Type, Type> CachedLooseSourceCollectionTypeBySourceCollectionType = [];
	static readonly Dictionary<Type, Dictionary<Type, CompiledInvokable>> CachedCollectionConvertorByDestinationTypeByLooseSourceType = [];
	public static TDest Convert<TSource,TDest>(TSource collection)
		where TSource: notnull
	=> (TDest) Convert(collection, typeof(TDest));

	public static object Convert(object collection, Type destinationType)
	{
		var collectionType = collection.GetType();
		if (collectionType == destinationType)
		{
			return collection;
		}
		if (destinationType == typeof(IDictionary))
		{
			return collectionType.GetCollectionItemType().IsKeyValuePairType(out var keyType, out var valueType)
				? Convert(collection, typeof(Dictionary<,>).MakeGenericType(keyType, valueType))
				: throw new InvalidOperationException();
		}
		if (destinationType == typeof(IEnumerable))
		{
			return collection;
		}

		var collectionConvertor = GetExistingOrBuildNewCollectionConvertor(collectionType, destinationType);
		var convertedCollection = collectionConvertor.Invoke(collection);
		return convertedCollection;
	}

	static CompiledInvokable GetExistingOrBuildNewCollectionConvertor(Type sourceCollectionType, Type destinationCollectionType)
	{
		var isSourceCollectionTypeKeyCached = CachedLooseSourceCollectionTypeBySourceCollectionType.TryGetValue(sourceCollectionType, out var looseSourceCollectionType);
		if (!isSourceCollectionTypeKeyCached)
		{
			looseSourceCollectionType = typeof(IEnumerable<>).MakeGenericType(sourceCollectionType.GetCollectionItemType());
		}
		var isLooseSourceCollectionTypeKeyCached = CachedCollectionConvertorByDestinationTypeByLooseSourceType.TryGetValue(looseSourceCollectionType!, out var cachedCollectionConvertorByDestinationType);
		if (!isLooseSourceCollectionTypeKeyCached)
		{
			cachedCollectionConvertorByDestinationType = [];
		}
		if (cachedCollectionConvertorByDestinationType!.TryGetValue(destinationCollectionType, out var cachedCollectionConvertor))
		{
			return cachedCollectionConvertor!;
		}

		var compiledConvertor = BuildCompiledCollectionConvertor(looseSourceCollectionType!, destinationCollectionType);
		cachedCollectionConvertorByDestinationType.Add(destinationCollectionType, compiledConvertor);

		if (!isSourceCollectionTypeKeyCached)
		{
			CachedLooseSourceCollectionTypeBySourceCollectionType.Add(sourceCollectionType, looseSourceCollectionType!);
		}
		if (!isLooseSourceCollectionTypeKeyCached)
		{
			CachedCollectionConvertorByDestinationTypeByLooseSourceType.TryAdd(sourceCollectionType, cachedCollectionConvertorByDestinationType);
		}


		return compiledConvertor;
	}

	static CompiledInvokable BuildCompiledCollectionConvertor(Type looseSourceCollectionType, Type destinationCollectionType)
	{
		var collectionParameterExpression = Expression.Parameter(looseSourceCollectionType, "source");
		var collectionConversionCallExpression = CollectionConversionCallExpressionBuilder.Build(collectionParameterExpression, destinationCollectionType);
		var collectionConversionLambdaExpression = Expression.Lambda(collectionConversionCallExpression, collectionParameterExpression);
		var compiledCollectionConvertor = CompiledInvokable.FromExpression(collectionConversionLambdaExpression);
		return compiledCollectionConvertor;
	}
}
