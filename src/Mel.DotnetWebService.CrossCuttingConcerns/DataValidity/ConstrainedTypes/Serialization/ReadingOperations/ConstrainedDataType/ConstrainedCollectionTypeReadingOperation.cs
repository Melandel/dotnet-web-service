using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataType;

class ConstrainedCollectionTypeReadingOperation : ConstrainedTypeReadingOperation
{
	static readonly ConstrainedCollectionTypeReadingOperation _instance = new();
	public static ConstrainedCollectionTypeReadingOperation Instance(Type targetType, Type instantiationOperationParameterType)
	{
		InstantiationOperationParameterTypesByTypeToConstruct.TryAdd(targetType, instantiationOperationParameterType);
		return _instance;
	}
	static readonly ConcurrentDictionary<Type, Type> InstantiationOperationParameterTypesByTypeToConstruct = [];
	ConstrainedCollectionTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instantiationParameterType = InstantiationOperationParameterTypesByTypeToConstruct[targetType];
		var collection = CollectionReadingOperation.Instance.Execute(ref reader, instantiationParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);
		var firstClassCollection = targetType.CreateInstanceUsingConstructorOrFactoryMethod(collection!, BindingFlags.Public);
		return firstClassCollection;
	}
}
