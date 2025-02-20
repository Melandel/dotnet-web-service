using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataTypeReadingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

abstract class ConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static ConstrainedTypeReadingOperation InstanceSuitedFor(Type type)
	=> type switch
	{
		//var t when t.IsAConstrainedDictionary(out var constructorParameterType, out var instanciationOperationParameterTypeCandidates) => ConstrainedDictionaryTypeReadingOperation.Instance(type, constructorParameterType, instanciationOperationParameterTypeCandidates),
		var t when t.IsAFirstClassCollectionWithIConstrainedCollectionOfKeyValuePairs(out var instanciationOperationParameterType)     => ConstrainedCollectionOfKeyValuePairsTypeReadingOperation.Instance(type, instanciationOperationParameterType),
		var t when t.IsAFirstClassCollectionWithIConstrainedCollection(out var instanciationOperationParameterType)                    => ConstrainedCollectionTypeReadingOperation.Instance(type, instanciationOperationParameterType),
		_ => ConstrainedScalarTypeReadingOperation.Instance
	};
}
