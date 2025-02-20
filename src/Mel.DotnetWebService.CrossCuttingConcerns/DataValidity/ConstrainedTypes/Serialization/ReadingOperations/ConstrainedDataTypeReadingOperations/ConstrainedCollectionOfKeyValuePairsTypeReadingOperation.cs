using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataTypeReadingOperations;

class ConstrainedCollectionOfKeyValuePairsTypeReadingOperation : ConstrainedTypeReadingOperation
{
	static readonly ConstrainedCollectionOfKeyValuePairsTypeReadingOperation _instance = new();
	public static ConstrainedCollectionOfKeyValuePairsTypeReadingOperation Instance(Type targetType, Type instanciationOperationParameterType)
	{
		InstanciationOperationParameterTypesByTypeToConstruct.TryAdd(targetType, instanciationOperationParameterType);
		return _instance;
	}
	static readonly Dictionary<Type, Type> InstanciationOperationParameterTypesByTypeToConstruct = [];
	ConstrainedCollectionOfKeyValuePairsTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instanciationParameterType = InstanciationOperationParameterTypesByTypeToConstruct[targetType];
		var collectionOfKeyValuePairs = KeyValuePairsReadingOperation.Instance.Execute(ref reader, instanciationParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);
		var firstClassCollectionOfKeyValuePairs = targetType.CreateInstanceUsingConstructorOrFactoryMethod(collectionOfKeyValuePairs!, BindingFlags.Public);
		return firstClassCollectionOfKeyValuePairs;
	}
}
