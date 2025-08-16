namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability;

public interface IConfigurationSettingsResolver
{
	void ResolveAllConfigurationSettings(string? configurationLocationMainIdentifier, string? configurationLocationFallbackIdentifier);
}
