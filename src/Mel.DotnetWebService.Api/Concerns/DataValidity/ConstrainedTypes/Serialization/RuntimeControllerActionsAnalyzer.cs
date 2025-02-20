using Mel.DotnetWebService.Api.Concerns.RuntimeValidation;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
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
	=> _controllerActionsExplorer.TypesInvolvedInControllerActionSignatures
		.Where(t => ConstrainedTypeInfos.Include(t))
		.ToHashSet();

	public HashSet<Type> ConstrainedTypesInvolvedInControllerActionSignaturesThatDontDefineHowToBeDeserialized
	=> ConstrainedTypesInvolvedInControllerActionSignatures
		.Where(t =>
		{
			ConstrainedTypeInfos.TryGet(t, out var constrainedTypeInfo);
			var correctlyDefinesHowToBeDeserialized = t switch
			{
				_ when constrainedTypeInfo.RootType.ImplementsInterface(typeof(IEnumerable<>)) => true,
				_ when t.ImplementsGenericInterface(typeof(IConstrainedValue<,>), out var argTypes) && argTypes.Last() == t => true,
				_ => false
			};

			return !correctlyDefinesHowToBeDeserialized;
		})
		.ToHashSet();
}
