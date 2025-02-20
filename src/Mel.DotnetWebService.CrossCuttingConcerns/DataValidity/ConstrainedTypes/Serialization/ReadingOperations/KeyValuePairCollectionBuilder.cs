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
	readonly Type _targetType;
	public Type KeyType { get; }
	public Type ValueType { get; }
	readonly ConstrainedTypeInfo? _constrainedKeyValuePairCollectionTypeInfo;
	KeyValuePairCollectionBuilder(
		dynamic keyValuePairCollection,
		Type targetType,
		KeyValuePairCollectionCategory kvpCollectionCategory,
		Type keyType,
		Type valueType,
		ConstrainedTypeInfo? constrainedKeyValuePairCollectionTypeInfo)
	{
		_keyValuePairCollection = keyValuePairCollection;
		_targetType = targetType;
		_kvpCollectionCategory = kvpCollectionCategory;
		KeyType = keyType;
		ValueType = valueType;
		_constrainedKeyValuePairCollectionTypeInfo = constrainedKeyValuePairCollectionTypeInfo;

	}
	public static KeyValuePairCollectionBuilder For(Type keyValuePairCollectionType)
	{
		var emptyCollection = CreateConcreteEmptyCollection(
			keyValuePairCollectionType,
			out var keyType,
			out var valueType,
			out var kvpCollectionCategory);

		var constrainedKeyValuePairCollectionTypeInfo = ConstrainedTypeInfos.TryGet(keyValuePairCollectionType, out var constrainedTypeInfo)
			? constrainedTypeInfo
			: null;

		return new(
			emptyCollection,
			keyValuePairCollectionType,
			kvpCollectionCategory,
			keyType,
			valueType,
			constrainedKeyValuePairCollectionTypeInfo);
	}

	static dynamic CreateConcreteEmptyCollection(Type targetType, out Type keyType, out Type valueType, out KeyValuePairCollectionCategory kvpCollectionCategory)
	{
		var concreteCollectionType = ComputeConcreteCollectionType(targetType, out keyType, out valueType, out kvpCollectionCategory);
		return Activator.CreateInstance(concreteCollectionType)!;
	}

	static void ComputeKeyAndValueTypes(Type targetType, out Type keyType, out Type valueType)
	{
		if (targetType.ImplementsGenericIEnumerableOfKeyPairValues(out keyType, out valueType))
		{
			return;
		}

		if (ConstrainedTypeInfos.TryGet(targetType, out var constrainedTypeInfo))
		{
			ComputeKeyAndValueTypes(constrainedTypeInfo.RootType, out keyType, out valueType);
			return;
		}

		throw new NotImplementedException();
	}
	static Type ComputeConcreteCollectionType(Type targetType, out Type keyType, out Type valueType, out KeyValuePairCollectionCategory kvpCollectionCategory)
	{
		ComputeKeyAndValueTypes(targetType, out keyType, out valueType);

		if (targetType.IsArray)
		{
			kvpCollectionCategory = KeyValuePairCollectionCategory.ListOfKeyValuePairs;
			return typeof(List<>).MakeGenericType(typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType));
		}

		if (targetType.IsInterface)
		{
			if (targetType.ImplementsGenericDictionary(out keyType, out valueType))
			{
				kvpCollectionCategory = KeyValuePairCollectionCategory.Dictionary;
				return typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
			}
			if (targetType.ImplementsGenericIEnumerableOfKeyPairValues(out keyType, out valueType))
			{
				kvpCollectionCategory = KeyValuePairCollectionCategory.ListOfKeyValuePairs;
				return typeof(List<>).MakeGenericType(typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType));
			}

			kvpCollectionCategory = KeyValuePairCollectionCategory.TechnicalDefaultEnumValue;
			throw new NotImplementedException($"{nameof(KeyValuePairCollectionBuilder)}.{nameof(CreateConcreteEmptyCollection)} does not handle interface type {targetType.GetName()}");
		}

		if (targetType.IsOrImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out var argumentTypes))
		{
			kvpCollectionCategory = KeyValuePairCollectionCategory.Dictionary;
			return typeof(Dictionary<,>).MakeGenericType(argumentTypes[0], argumentTypes[1]);
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
		if (_constrainedKeyValuePairCollectionTypeInfo != null)
		{
			return _constrainedKeyValuePairCollectionTypeInfo.InvokeStaticFactoryMethod(_keyValuePairCollection);
		}

		if (_targetType.IsArray)
		{
			return _keyValuePairCollection.ToArray();
		}

		return _keyValuePairCollection;
	}
}
