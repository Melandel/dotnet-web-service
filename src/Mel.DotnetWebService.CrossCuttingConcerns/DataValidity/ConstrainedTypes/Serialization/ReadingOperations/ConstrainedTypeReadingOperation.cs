using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.ConstrainedDataType;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

abstract class ConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static ConstrainedTypeReadingOperation InstanceSuitedFor(Type type)
	=> type switch
	{
		var t when t.IsANonGenericImplementationOfIConstrainedCollectionOfKeyValuePairs(out var instantiationOperationParameterType)     => ConstrainedCollectionOfKeyValuePairsTypeReadingOperation.Instance(type, instantiationOperationParameterType),
		var t when t.IsANonGenericImplementationOfIConstrainedCollection(out var instantiationOperationParameterType)                    => ConstrainedCollectionTypeReadingOperation.Instance(type, instantiationOperationParameterType),
		_ => ConstrainedScalarTypeReadingOperation.Instance
	};
}
