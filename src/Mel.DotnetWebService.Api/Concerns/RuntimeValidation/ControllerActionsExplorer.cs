using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Mel.DotnetWebService.Api.Concerns.RuntimeValidation;

class ControllerActionsExplorer
{
	readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
	public ControllerActionsExplorer(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
	{
		_actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
	}

	public NonEmptyHashSet<Type> TypesPresentInControllerActionSignatures
	{
		get
		{
			var typesInEndpointSignatures = new HashSet<Type>();
			var controllerActions = _actionDescriptorCollectionProvider.ActionDescriptors.Items
				.Where(actionDescriptor => actionDescriptor is ControllerActionDescriptor)
				.Cast<ControllerActionDescriptor>()
				.ToArray();
			foreach (var controllerAction in controllerActions)
			{
				var method = controllerAction.MethodInfo;
				typesInEndpointSignatures.Add(method.ReturnType);
				foreach (var parameter in method.GetParameters())
				{
					typesInEndpointSignatures.Add(parameter.ParameterType);
				}
			}

			return NonEmptyHashSet.Storing<Type>(typesInEndpointSignatures);
		}
	}

	public NonEmptyHashSet<Type> TypesInvolvedInControllerActionSignatures
	{
		get
		{
			var typesAndSubTypes = new HashSet<Type>();

			foreach (var type in TypesPresentInControllerActionSignatures)
			{
				type.HasFieldOrPropertyWithConstrainedType(browseRecursively: true, typesAndSubTypes);
			}

			return NonEmptyHashSet.Storing<Type>(typesAndSubTypes);
		}
	}

	public HashSet<Type> TypesInvolvedInControllerActionSignaturesThatAreConstrainedTypes
	{
		get
		{
			var v = TypesInvolvedInControllerActionSignatures.Where(t => t.IsAConstrainedType()).ToHashSet();
			return v;
		}
	}
}
