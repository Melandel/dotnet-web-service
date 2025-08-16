using Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;
using Mel.DotnetWebService.CrossCuttingConcerns.Configurability.SingleConfigurationSettingRedirectingToTheFullConfiguration;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability;

public static class ConfigurationManagerExtensionMethods
{
	public static bool HasAlreadyBeenFullyResolved(this ConfigurationManager configurationManager)
	=> configurationManager[nameof(ConfigurationSettingsResolutionStatus)]
		.Matches(ConfigurationSettingsResolutionStatus.FullyResolved);

	public static void ResolveAllSettings(
		this ConfigurationManager configurationManager,
		IHostEnvironment hostEnvironment,
		string mainConfigurationLocationIdKey,
		string fallbackConfigurationLocationIdKey,
		NonEmptyHashSet<ConfigurationKeyFilter> configurationKeyFilters)
	{
		var deploymentEnvironment = DeploymentEnvironment.ThatMatches(hostEnvironment.EnvironmentName, defaultValue: DeploymentEnvironment.Development);
		var mainConfigurationLocationId = deploymentEnvironment.ApplySuffixToConfigurationFilePath(configurationManager[mainConfigurationLocationIdKey]);
		var fallbackConfigurationLocationId = deploymentEnvironment.ApplySuffixToConfigurationFilePath(configurationManager[fallbackConfigurationLocationIdKey]);

		new ConfigurationSettingsResolver(configurationManager)
			.ResolveAllConfigurationSettings(
				mainConfigurationLocationId,
				fallbackConfigurationLocationId,
				configurationKeyFilters);
	}
}
