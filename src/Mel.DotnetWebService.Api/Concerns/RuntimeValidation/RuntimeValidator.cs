namespace Mel.DotnetWebService.Api.Concerns.RuntimeValidation;

class RuntimeValidator
{
	public ControllerActionsExplorer ControllerActionsExplorer { get; }

	public RuntimeValidator(ControllerActionsExplorer controllerActionsExplorer)
	{
		ControllerActionsExplorer = controllerActionsExplorer;
	}
}
