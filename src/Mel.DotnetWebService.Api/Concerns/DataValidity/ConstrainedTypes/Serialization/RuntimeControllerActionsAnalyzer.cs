using Mel.DotnetWebService.Api.Concerns.RuntimeValidation;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.Serialization;

class RuntimeControllerActionsAnalyzer
{
	readonly ControllerActionsExplorer _controllerActionsExplorer;
	public RuntimeControllerActionsAnalyzer(ControllerActionsExplorer controllerActionsExplorer)
	{
		_controllerActionsExplorer = controllerActionsExplorer;
	}

	public HashSet<Type> ConstrainedTypesInvolvedInControllerActionSignatures
	=> _controllerActionsExplorer.TypesInvolvedInControllerActionSignatures.Where(t => t.IsAConstrainedType()).ToHashSet();

	public HashSet<Type> ConstrainedTypesInvolvedInControllerActionSignaturesThatDontDefineHowToBeDeserialized
	=> ConstrainedTypesInvolvedInControllerActionSignatures
		.Where(t =>
		{
			var correctlyDefinesHowToBeDeserialized = t switch
			{
				_ when t.GetConstrainedTypeRootType().ImplementsInterface(typeof(IEnumerable<>)) => true,
				_ when t.ImplementsGenericInterface(typeof(ICanBeDeserializedFromJson<,>), out var argTypes) && argTypes.Last() == t => true,
				_ => false
			};

			return !correctlyDefinesHowToBeDeserialized;
		})
		.ToHashSet();
}
