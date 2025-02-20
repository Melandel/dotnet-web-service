using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations;

abstract class TypeInvolvingConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static TypeInvolvingConstrainedTypeReadingOperation InstanceSuitedFor(Type type)
	=> type switch
	{
		var t when t.IsAFirstClassCollection(out var constructorParameterType, out var instanciationOperationParameterTypeCandidates) => FirstClassCollectionReadingOperation.Instance(type, constructorParameterType, instanciationOperationParameterTypeCandidates),
		var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor) => PositionalRecordReadingOperation.Instance(type, propertyBasedConstructor!),
		var t when t.IsDeclaredAsAGetSetStyleClass() => GetSetStyleClassReadingOperation.Instance,
		_ => ClassWithConstructorAndSettersReadingOperation.Instance
	};
}
