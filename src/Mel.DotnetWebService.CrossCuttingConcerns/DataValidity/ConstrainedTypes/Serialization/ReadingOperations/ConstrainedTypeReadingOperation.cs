using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataTypeReadingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

abstract class ConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static ConstrainedTypeReadingOperation InstanceSuitedFor(Type type)
	=> type switch
	{
		var t when t.IsANonGenericImplementationOfIConstrainedCollectionOfKeyValuePairs(out var instanciationOperationParameterType)     => ConstrainedCollectionOfKeyValuePairsTypeReadingOperation.Instance(type, instanciationOperationParameterType),
		var t when t.IsANonGenericImplementationOfIConstrainedCollection(out var instanciationOperationParameterType)                    => ConstrainedCollectionTypeReadingOperation.Instance(type, instanciationOperationParameterType),
		_ => ConstrainedScalarTypeReadingOperation.Instance
	};
}
