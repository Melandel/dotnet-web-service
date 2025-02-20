using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

class NonGenericConstrainedCollectionReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly NonGenericConstrainedCollectionReadingOperation _instance = new();
	static readonly ConcurrentDictionary<Type, Type> InstantiationOperationParameterTypesByTypeToConstruct = new ConcurrentDictionary<Type, Type>();
	NonGenericConstrainedCollectionReadingOperation()
	{
	}

	public static NonGenericConstrainedCollectionReadingOperation Instance(Type targetType, Type instantiationOperationParameterType)
	{
		InstantiationOperationParameterTypesByTypeToConstruct.TryAdd(targetType, instantiationOperationParameterType);
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instantiationParameterType = InstantiationOperationParameterTypesByTypeToConstruct[targetType];
		var collection = CollectionReadingOperation.Instance.Execute(ref reader, instantiationParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);
		var firstClassCollection = targetType.CreateInstanceUsingConstructorOrFactoryMethod(collection!, BindingFlags.Public);
		return firstClassCollection;
	}
}
