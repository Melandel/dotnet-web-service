using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectInstantiation;

class GenericKeyValuePairBuilder
{
	object? _key;
	object? _value;
	readonly Type _keyType;
	readonly Type _valueType;
	static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, Func<object, object?, object>>> KeyValuePairInstantiatorByKeyAndValueTypes = [];
	GenericKeyValuePairBuilder(Type keyType, Type valueType)
	{
		_keyType = keyType;
		_valueType = valueType;
	}
	public static GenericKeyValuePairBuilder ForTypes(Type keyType, Type valueType)
	{
		if (!KeyValuePairInstantiatorByKeyAndValueTypes.TryGetValue(keyType, out var keyValuePairInstantiatorByValueType))
		{
			keyValuePairInstantiatorByValueType = new ConcurrentDictionary<Type, Func<object, object?, object>>();
			KeyValuePairInstantiatorByKeyAndValueTypes.TryAdd(keyType, keyValuePairInstantiatorByValueType);
		}
		if (!keyValuePairInstantiatorByValueType.TryGetValue(valueType, out var keyValuePairInstantiator))
		{
			keyValuePairInstantiator = BuildKeyValuePairInstantiationOperation(keyType, valueType);
			keyValuePairInstantiatorByValueType.TryAdd(valueType, keyValuePairInstantiator);
		}

		return new(keyType, valueType);
	}

	public GenericKeyValuePairBuilder WithKeyAndValue(object key, object? value)
	=> WithKey(key).WithValue(value);

	public GenericKeyValuePairBuilder WithKey(object key)
	{
		_key = key;
		return this;
	}

	public GenericKeyValuePairBuilder WithValue(object? value)
	{
		_value = value;
		return this;
	}

	public dynamic BuildAsDynamic()
	{
		var kvpInstantiationOperation = KeyValuePairInstantiatorByKeyAndValueTypes[_keyType][_valueType];
		var kvp = kvpInstantiationOperation.Invoke(_key!, _value);
		return kvp;
	}

	static Func<object, object?, object> BuildKeyValuePairInstantiationOperation(Type keyType, Type valueType)
	{
		var key = Expression.Parameter(typeof(object), "key");
		var value = Expression.Parameter(typeof(object), "value");

		var pairType = typeof(KeyValuePair<,>)
			.MakeGenericType(keyType, valueType);

		var constructor = pairType.GetConstructor(
			new[] { keyType, valueType })
			?? throw new InvalidOperationException(
				$"Constructor not found for {pairType}.");

		var newPair = Expression.New(
			constructor,
			Expression.Convert(key, keyType),
			Expression.Convert(value, valueType));

		var body = Expression.Convert(newPair, typeof(object));

		return Expression.Lambda<Func<object, object?, object>>(
			body,
			key,
			value)
			.Compile();
	}
}
