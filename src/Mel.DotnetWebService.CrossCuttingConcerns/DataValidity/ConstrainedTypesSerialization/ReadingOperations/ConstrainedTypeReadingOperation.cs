using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.ConstrainedDataTypeReadingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations;

abstract class ConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static ConstrainedTypeReadingOperation InstanceSuitedFor(Type type)
	=> type switch
	{
		var t when t.IsAFirstClassCollection(out var constructorParameterType, out var instanciationOperationParameterTypeCandidates) => ConstrainedCollectionTypeReadingOperation.Instance(type, constructorParameterType, instanciationOperationParameterTypeCandidates),
		_ => ConstrainedScalarTypeReadingOperation.Instance
	};
}
