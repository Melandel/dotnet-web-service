using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataType;

class ConstrainedCollectionOfKeyValuePairsTypeReadingOperation : ConstrainedTypeReadingOperation
{
	static readonly ConstrainedCollectionOfKeyValuePairsTypeReadingOperation _instance = new();
	public static ConstrainedCollectionOfKeyValuePairsTypeReadingOperation Instance(Type targetType, Type instantiationOperationParameterType)
	{
		InstantiationOperationParameterTypesByTypeToConstruct.TryAdd(targetType, instantiationOperationParameterType);
		return _instance;
	}
	static readonly ConcurrentDictionary<Type, Type> InstantiationOperationParameterTypesByTypeToConstruct = [];
	ConstrainedCollectionOfKeyValuePairsTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instantiationParameterType = InstantiationOperationParameterTypesByTypeToConstruct[targetType];
		var collectionOfKeyValuePairs = KeyValuePairsReadingOperation.Instance.Execute(ref reader, instantiationParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);
		var firstClassCollectionOfKeyValuePairs = targetType.CreateInstanceUsingConstructorOrFactoryMethod(collectionOfKeyValuePairs!, BindingFlags.Public);
		return firstClassCollectionOfKeyValuePairs;
	}
}
