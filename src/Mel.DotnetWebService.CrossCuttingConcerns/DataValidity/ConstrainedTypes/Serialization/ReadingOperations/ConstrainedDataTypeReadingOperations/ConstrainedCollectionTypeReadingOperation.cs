using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataTypeReadingOperations;

class ConstrainedCollectionTypeReadingOperation : ConstrainedTypeReadingOperation
{
	static readonly ConstrainedCollectionTypeReadingOperation _instance = new();
	public static ConstrainedCollectionTypeReadingOperation Instance(Type targetType, Type instanciationOperationParameterType)
	{
		InstanciationOperationParameterTypesByTypeToConstruct.TryAdd(targetType, instanciationOperationParameterType);
		return _instance;
	}
	static readonly Dictionary<Type, Type> InstanciationOperationParameterTypesByTypeToConstruct = [];
	ConstrainedCollectionTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instanciationParameterType = InstanciationOperationParameterTypesByTypeToConstruct[targetType];
		var collection = CollectionReadingOperation.Instance.Execute(ref reader, instanciationParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);
		var firstClassCollection = targetType.CreateInstanceUsingConstructorOrFactoryMethod(collection!, BindingFlags.Public);
		return firstClassCollection;
	}
}
