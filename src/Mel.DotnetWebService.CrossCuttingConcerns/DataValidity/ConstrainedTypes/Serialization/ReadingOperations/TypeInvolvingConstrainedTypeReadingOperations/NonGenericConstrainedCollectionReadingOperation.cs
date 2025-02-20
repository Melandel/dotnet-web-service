using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

class NonGenericConstrainedCollectionReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	static readonly NonGenericConstrainedCollectionReadingOperation _instance = new();
	static readonly Dictionary<Type, Type> InstanciationOperationParameterTypesByTypeToConstruct = new Dictionary<Type, Type>();
	NonGenericConstrainedCollectionReadingOperation()
	{
	}

	public static NonGenericConstrainedCollectionReadingOperation Instance(Type targetType, Type instanciationOperationParameterType)
	{
		InstanciationOperationParameterTypesByTypeToConstruct.TryAdd(targetType, instanciationOperationParameterType);
		return _instance;
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var instanciationParameterType = InstanciationOperationParameterTypesByTypeToConstruct[targetType];
		var collection = CollectionReadingOperation.Instance.Execute(ref reader, instanciationParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);
		var firstClassCollection = targetType.CreateInstanceUsingConstructorOrFactoryMethod(collection!, BindingFlags.Public);
		return firstClassCollection;
	}
}
