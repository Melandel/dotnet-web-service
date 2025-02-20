using System.Collections;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class GenericListOfKeyValuePairsBuilder
{
	readonly GenericListBuilder _keyValuePairsBuilder;
	readonly Type _keyType;
	readonly Type _valueType;
	GenericListOfKeyValuePairsBuilder(GenericListBuilder keyValuePairsBuilder, Type keyType, Type valueType)
	{
		_keyValuePairsBuilder = keyValuePairsBuilder;
		_keyType = keyType;
		_valueType = valueType;
	}

	public static GenericListOfKeyValuePairsBuilder For(Type keyType, Type valueType)
	{
		var keyValuePairType = typeof(KeyValuePair<,>).MakeGenericType([ keyType, valueType ]);
		var keyValuePairsBuilder = GenericListBuilder.For(keyValuePairType);
		return new(keyValuePairsBuilder, keyType, valueType);
	}

	public GenericListOfKeyValuePairsBuilder Add(object key, object? value)
	{
		var kvp = GenericKeyValuePairBuilder
			.ForTypes(_keyType, _valueType)
			.WithKeyAndValue(key, value)
			.BuildAsDynamic();
		_keyValuePairsBuilder.Add(kvp);
		return this;
	}

	public dynamic BuildAsDynamic()
	=> BuildAsIDictionary();

	public IDictionary BuildAsIDictionary()
	{
		var kvpsAsIList = BuildAsIList();
		return (IDictionary) CollectionConverter.Convert(kvpsAsIList, typeof(IDictionary));
	}

	public IList BuildAsIList()
	=> _keyValuePairsBuilder.BuildAsIList();
}
