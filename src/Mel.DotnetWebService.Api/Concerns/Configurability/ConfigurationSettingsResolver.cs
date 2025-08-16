using System.Configuration;
using Mel.DotnetWebService.CrossCuttingConcerns.Configurability;

namespace Mel.DotnetWebService.Api.Concerns.Configurability;

class ConfigurationSettingsResolver : IConfigurationSettingsResolver
{
	readonly IConfigurationBuilder _configurationBuilder;
	readonly static JsonConfigurationFilter[] JsonConfigurationFilters = new[]
	{
		new JsonConfigurationFilter(Prefix: "Private", RemovePrefixFromConfigurationItemKey: true),
		new JsonConfigurationFilter(Prefix: "Public", RemovePrefixFromConfigurationItemKey: false)
	};
	public ConfigurationSettingsResolver(IConfigurationBuilder configurationBuilder)
	{
		_configurationBuilder = configurationBuilder;
	}

	public void ResolveAllConfigurationSettings(string? configurationLocationMainIdentifier, string? configurationLocationFallbackIdentifier)
	{
		if (string.IsNullOrEmpty(configurationLocationMainIdentifier))
		{
			throw new ConfigurationErrorsException($"Could not find the following configuration setting: {nameof(configurationLocationMainIdentifier)}");
		}

		try
		{
			_configurationBuilder.AddJsonFile(
				configurationLocationMainIdentifier,
				JsonConfigurationFilters);
		}
		catch (Exception e)
		{
			if (string.IsNullOrEmpty(configurationLocationFallbackIdentifier))
			{
				throw new ConfigurationErrorsException($"Could not load the configuration located at {nameof(configurationLocationMainIdentifier)}", e);
			}

			try
			{
				_configurationBuilder.AddJsonFile(
					configurationLocationMainIdentifier,
					JsonConfigurationFilters);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException($"Could not load neither the configuration located at {nameof(configurationLocationMainIdentifier)}, neither the configuration located at {nameof(configurationLocationFallbackIdentifier)}", ex);
			}
		}
	}
}
