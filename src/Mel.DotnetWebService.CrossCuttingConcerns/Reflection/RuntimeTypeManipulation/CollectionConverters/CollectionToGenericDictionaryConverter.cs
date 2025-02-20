using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class CollectionToGenericDictionaryConverter : CollectionConverter
{
	static readonly Dictionary<Type, Dictionary<Type, Func>> CollectionToDictionaryConversionByKeyAndValueTypes = [];
	public static readonly CollectionToGenericDictionaryConverter Instance = new();

	public override object ConvertCollection(object collection, Type destinationType)
	{
		if (IsContrainedCollectionOfKeyValuePairs(destinationType))
		{
			ConstrainedTypeInfo constrainedTypeInfo = destinationType switch
			{
				var t when t.IsGenericType && ConstrainedTypeInfos.TryGet(destinationType.GetGenericTypeDefinition(), out var constrainedGenericTypeInfo) => constrainedGenericTypeInfo,
				var t when !t.IsGenericType && ConstrainedTypeInfos.TryGet(destinationType, out var constrainedNonGenericTypeInfo) => constrainedNonGenericTypeInfo,
				_ => throw new InvalidOperationException()
			};

			return constrainedTypeInfo
				.InvokeStaticFactoryMethod(
					ConvertCollection(collection, constrainedTypeInfo.RootType));
		}

		if (IsIEnumerableOfKeyValuePairs(collection.GetType(), out var sourceKeyType, out var sourceValueType))
		{
			if (!CollectionToDictionaryConversionByKeyAndValueTypes.TryGetValue(sourceKeyType!, out var collectionToDictionaryConversionByValueType))
			{
				collectionToDictionaryConversionByValueType = new Dictionary<Type, Func>();
				CollectionToDictionaryConversionByKeyAndValueTypes.Add(sourceKeyType!, collectionToDictionaryConversionByValueType);
			}
			if (!collectionToDictionaryConversionByValueType.TryGetValue(sourceValueType!, out var collectionToDictionaryConversion))
			{
				// destination == IDictionary && sourceKeyType == destinationKeyType && sourceValueType == destinationKeyType
				// destination implements IEnumerable<souceKeyType, sourceValueType> && sourceKeyType == destinationKeyType && sourceValueType == destinationKeyType
				collectionToDictionaryConversion = BuildGenericIEnumerableToDictionaryConversionOperation(sourceKeyType!, sourceValueType!);
				collectionToDictionaryConversionByValueType.Add(sourceValueType!, collectionToDictionaryConversion!);
			}

			var dictionary = collectionToDictionaryConversion!.Invoke(collection);
			return dictionary;
		}

		throw new NotImplementedException($"foo {collection.GetType().GetName()} -> {destinationType.GetName()}");
	}

	static bool IsContrainedCollectionOfKeyValuePairs(Type destinationType)
	=> destinationType.IsOrImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out var argTypes);

	static bool IsIEnumerableOfKeyValuePairs(Type collectionType, out Type? keyType, out Type? valueType)
	{
		if (!collectionType.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes))
		{
			keyType = null;
			valueType = null;
			return false;
		}
		var kvpType = argTypes[0];
		if (!kvpType.IsGenericType || kvpType.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
		{
			keyType = null;
			valueType = null;
			return false;
		}
		var keyAndValueTypes = kvpType.GetGenericArguments();
		keyType = keyAndValueTypes.First();
		valueType = keyAndValueTypes.Last();
		return true;
	}

	static Func BuildGenericIEnumerableToDictionaryConversionOperation(Type keyType, Type valueType)
	=> BuildToDictionaryConversionOperation(keyType, valueType, nameof(GenericIEnumerableToDictionary));

	static Func BuildICollectionToDictionaryConversionOperation(Type keyType, Type valueType)
	=> BuildToDictionaryConversionOperation(keyType, valueType, nameof(ICollectionToDictionaryGeneric));

	static Func BuildIEnumerableToDictionaryConversionOperation(Type keyType, Type valueType)
	=> BuildToDictionaryConversionOperation(keyType, valueType, nameof(IEnumerableToDictionaryGeneric));

	static Func BuildToDictionaryConversionOperation(Type keyType, Type valueType, string methodName)
	{
		var iCollectionToDictionaryMethod = typeof(CollectionToGenericDictionaryConverter)
			.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
			.First(m => string.Equals(m.Name, methodName) &&	m.IsGenericMethodDefinition)
			.MakeGenericMethod(new[] { keyType, valueType });
		return Func.CompileCallToStaticMethod(iCollectionToDictionaryMethod);
	}

	static Dictionary<TKey, TValue> GenericIEnumerableToDictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> kvpEnumerable)
		where TKey: notnull
	{
		return kvpEnumerable
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
	}

	static Dictionary<TKey, TValue> ICollectionToDictionaryGeneric<TKey, TValue>(ICollection collection)
		where TKey: notnull
	{
		var result = new Dictionary<TKey, TValue>(collection.Count);
		foreach (KeyValuePair<TKey, TValue> pair in collection)
		{
			result.Add(pair.Key, pair.Value);
		}
		return result;
	}

	static Dictionary<TKey, TValue> IEnumerableToDictionaryGeneric<TKey, TValue>(IEnumerable enumerable)
		where TKey: notnull
	{
		var result = new Dictionary<TKey, TValue>();
		foreach (KeyValuePair<TKey, TValue> pair in enumerable)
		{
			result.Add(pair.Key, pair.Value);
		}
		return result;
	}
}

