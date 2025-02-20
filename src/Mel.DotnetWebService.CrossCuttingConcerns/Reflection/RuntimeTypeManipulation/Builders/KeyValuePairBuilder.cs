using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class GenericKeyValuePairBuilder
{
	object? _key;
	object? _value;
	readonly Type _keyType;
	readonly Type _valueType;
	static readonly Dictionary<Type, Dictionary<Type, Func<object, object?, object>>> KeyValuePairInstanciatorByKeyAndValueTypes = [];
	GenericKeyValuePairBuilder(Type keyType, Type valueType)
	{
		_keyType = keyType;
		_valueType = valueType;
	}
	public static GenericKeyValuePairBuilder ForTypes(Type keyType, Type valueType)
	{
		if (!KeyValuePairInstanciatorByKeyAndValueTypes.TryGetValue(keyType, out var keyValuePairInstanciatorByValueType))
		{
			keyValuePairInstanciatorByValueType = new Dictionary<Type, Func<object, object?, object>>();
			KeyValuePairInstanciatorByKeyAndValueTypes.Add(keyType, keyValuePairInstanciatorByValueType);
		}
		if (!keyValuePairInstanciatorByValueType.TryGetValue(valueType, out var keyValuePairInstanciator))
		{
			keyValuePairInstanciator = BuildKeyValuePairInstanciationOperation(keyType, valueType);
			keyValuePairInstanciatorByValueType.Add(valueType, keyValuePairInstanciator);
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
		var kvpInstanciationOperation = KeyValuePairInstanciatorByKeyAndValueTypes[_keyType][_valueType];
		var kvp = kvpInstanciationOperation.Invoke(_key!, _value);
		return kvp;
	}

	static Func<object, object?, object> BuildKeyValuePairInstanciationOperation(Type keyType, Type valueType)
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
