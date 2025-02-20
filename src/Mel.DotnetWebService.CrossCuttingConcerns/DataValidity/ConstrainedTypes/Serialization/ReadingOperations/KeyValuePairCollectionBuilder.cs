using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

class KeyValuePairCollectionBuilder
{
	readonly KeyValuePairCollectionCategory _kvpCollectionCategory;
	enum KeyValuePairCollectionCategory
	{
		TechnicalDefaultEnumValue = 0,
		Dictionary = 1,
		ListOfKeyValuePairs = 2
	}

	readonly dynamic _keyValuePairCollection;
	public Type KeyType { get; }
	public Type ValueType { get; }
	readonly ConstrainedTypeInfo? _constrainedKeyValuePairCollectionTypeInfo;
	KeyValuePairCollectionBuilder(
		dynamic keyValuePairCollection,
		KeyValuePairCollectionCategory kvpCollectionCategory,
		Type keyType,
		Type valueType,
		ConstrainedTypeInfo? constrainedKeyValuePairCollectionTypeInfo)
	{
		_keyValuePairCollection = keyValuePairCollection;
		_kvpCollectionCategory = kvpCollectionCategory;
		KeyType = keyType;
		ValueType = valueType;
		_constrainedKeyValuePairCollectionTypeInfo = constrainedKeyValuePairCollectionTypeInfo;

	}
	public static KeyValuePairCollectionBuilder For(Type keyValuePairCollectionType)
	{
		var emptyCollection = CreateConcreteEmptyCollection(
			keyValuePairCollectionType,
			out var concreteCollectionType,
			out var keyType,
			out var valueType,
			out var kvpCollectionCategory);

		var constrainedKeyValuePairCollectionTypeInfo = keyValuePairCollectionType switch
		{
			var t when ConstrainedTypeInfos.TryGet(t, out var constrainedTypeInfo) => constrainedTypeInfo,
			var t when ConstrainedTypeInfos.TryGet(t.GetGenericTypeDefinition(), out var constrainedTypeInfo) => constrainedTypeInfo,
			_ => null
		};
		return new(
			emptyCollection,
			kvpCollectionCategory,
			keyType,
			valueType,
			constrainedKeyValuePairCollectionTypeInfo);
	}

	static dynamic CreateConcreteEmptyCollection(Type targetType, out Type concreteCollectionType, out Type keyType, out Type valueType, out KeyValuePairCollectionCategory kvpCollectionCategory)
	{
		concreteCollectionType = ComputeConcreteCollectionType(targetType, out keyType, out valueType, out kvpCollectionCategory);
		return Activator.CreateInstance(concreteCollectionType)!;
	}

	static Type ComputeConcreteCollectionType(Type targetType, out Type keyType, out Type valueType, out KeyValuePairCollectionCategory kvpCollectionCategory)
	{
		var dictionaryGenericArgs = targetType.GetGenericArguments();
		keyType = dictionaryGenericArgs.First();
		valueType = dictionaryGenericArgs.Last();

		if (targetType.IsInterface)
		{
			if (targetType.ImplementsGenericDictionary(out keyType, out valueType))
			{
				kvpCollectionCategory = KeyValuePairCollectionCategory.Dictionary;
				return typeof(Dictionary<,>).MakeGenericType(new Type[] { keyType, valueType });
			}
			if (targetType.ImplementsGenericIEnumerableOfKeyPairValues(out keyType, out valueType))
			{
				kvpCollectionCategory = KeyValuePairCollectionCategory.ListOfKeyValuePairs;
				return typeof(List<>).MakeGenericType(typeof(KeyValuePair<,>).MakeGenericType(new Type[] { keyType, valueType }));
			}

			kvpCollectionCategory = KeyValuePairCollectionCategory.TechnicalDefaultEnumValue;
			throw new NotImplementedException($"{nameof(KeyValuePairCollectionBuilder)}.{nameof(CreateConcreteEmptyCollection)} does not handle interface type {targetType.GetName()}");
		}

		if (targetType.IsOrImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out var argumentTypes))
		{
			kvpCollectionCategory = KeyValuePairCollectionCategory.Dictionary;
			return typeof(Dictionary<,>).MakeGenericType(new[] { argumentTypes[0], argumentTypes[1] });
		}

		kvpCollectionCategory = targetType.ImplementsGenericDictionary(out _, out _)
			? KeyValuePairCollectionCategory.Dictionary
			: KeyValuePairCollectionCategory.ListOfKeyValuePairs;
		return targetType;
	}

	public KeyValuePairCollectionBuilder Add(dynamic key, dynamic value)
	{
		switch (_kvpCollectionCategory)
		{
			case KeyValuePairCollectionCategory.Dictionary:
				_keyValuePairCollection.Add(key, value);
				break;
			case KeyValuePairCollectionCategory.ListOfKeyValuePairs:
				_keyValuePairCollection.Add(KeyValuePair.Create(key, value));
				break;
			default:
				throw new InvalidOperationException();
		}

		return this;
	}

	public dynamic Build()
	{
		return (_constrainedKeyValuePairCollectionTypeInfo != null)
			? _constrainedKeyValuePairCollectionTypeInfo.InvokeStaticFactoryMethod(_keyValuePairCollection)
			: _keyValuePairCollection;
	}
}
