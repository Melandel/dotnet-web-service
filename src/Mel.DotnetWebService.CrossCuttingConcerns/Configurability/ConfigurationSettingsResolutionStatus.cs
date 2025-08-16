namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability;

class ConfigurationSettingsResolutionStatus
{
	readonly string _name;
	ConfigurationSettingsResolutionStatus(string name)
	{
		_name = name;
	}
	public static readonly ConfigurationSettingsResolutionStatus FullyResolved = new(nameof(FullyResolved));

	public static implicit operator string(ConfigurationSettingsResolutionStatus status) => status._name;
}
