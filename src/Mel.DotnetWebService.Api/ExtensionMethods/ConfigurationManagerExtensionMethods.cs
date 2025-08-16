using Mel.DotnetWebService.Api.Concerns.Configurability;
using Mel.DotnetWebService.CrossCuttingConcerns.Configurability;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ConfigurationManagerExtensionMethods
{
	public static bool HasAlreadyBeenFullyResolved(this ConfigurationManager configurationManager)
	=> configurationManager[nameof(ConfigurationSettingsResolutionStatus)]
		.Matches(ConfigurationSettingsResolutionStatus.FullyResolved);

	public static void ResolveAllSettings(this ConfigurationManager configurationManager, IHostEnvironment hostEnvironment, string mainConfigurationLocationIdKey, string fallbackConfigurationLocationIdKey)
	{
		var deploymentEnvironment = DeploymentEnvironment.ThatMatches(hostEnvironment.EnvironmentName, defaultValue: DeploymentEnvironment.Development);
		var mainConfigurationLocationId     = deploymentEnvironment.ApplySuffixToConfigurationFilePath(configurationManager[mainConfigurationLocationIdKey]);
		var fallbackConfigurationLocationId = deploymentEnvironment.ApplySuffixToConfigurationFilePath(configurationManager[fallbackConfigurationLocationIdKey]);

		new ConfigurationSettingsResolver(configurationManager)
			.ResolveAllConfigurationSettings(
				mainConfigurationLocationId,
				fallbackConfigurationLocationId);
	}
}
