using System.Configuration;
using Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;
using Microsoft.Extensions.Configuration;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability.SingleConfigurationSettingRedirectingToTheFullConfiguration;

class ConfigurationSettingsResolver : IConfigurationSettingsResolver
{
	readonly IConfigurationBuilder _configurationBuilder;
	public ConfigurationSettingsResolver(IConfigurationBuilder configurationBuilder)
	{
		_configurationBuilder = configurationBuilder;
	}

	public void ResolveAllConfigurationSettings(string? configurationLocationMainIdentifier, string? configurationLocationFallbackIdentifier, NonEmptyHashSet<ConfigurationKeyFilter> configurationKeyFilters)
	{
		if (string.IsNullOrEmpty(configurationLocationMainIdentifier))
		{
			throw new ConfigurationErrorsException($"Could not find the following configuration setting: {nameof(configurationLocationMainIdentifier)}");
		}

		try
		{
			_configurationBuilder.AddFilteredJsonFile(configurationLocationMainIdentifier, configurationKeyFilters);
		}
		catch (Exception e)
		{
			if (string.IsNullOrEmpty(configurationLocationFallbackIdentifier))
			{
				throw new ConfigurationErrorsException($"Could not load the configuration located at {nameof(configurationLocationMainIdentifier)}", e);
			}

			try
			{
				_configurationBuilder.AddFilteredJsonFile(configurationLocationMainIdentifier, configurationKeyFilters);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException($"Could not load neither the configuration located at {nameof(configurationLocationMainIdentifier)}, neither the configuration located at {nameof(configurationLocationFallbackIdentifier)}", ex);
			}
		}
	}
}
