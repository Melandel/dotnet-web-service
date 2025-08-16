using Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability.JsonConfigurationFile;

class FilteredJsonConfigurationProvider : JsonConfigurationProvider
{
	readonly NonEmptyHashSet<ConfigurationKeyFilter> _keyFilters;
	public FilteredJsonConfigurationProvider(FilteredJsonConfigurationSource source, NonEmptyHashSet<ConfigurationKeyFilter> keyFilters)
	: base(source) => _keyFilters = keyFilters;

	public override void Load(Stream stream)
	{
		base.Load(stream);
		var filteredConfigurationSettings = FilterSettings(Data, _keyFilters);
		Data = filteredConfigurationSettings;
	}

	Dictionary<string, string?> FilterSettings(IDictionary<string, string?> settings, NonEmptyHashSet<ConfigurationKeyFilter> configurationKeyFilters)
	{
		var keyFiltersFromLongestPrefixToShortest = configurationKeyFilters.OrderByDescending(keyFilter => keyFilter.Prefix.Length);

		var filteredConfigurationSettings = new Dictionary<string, string?>();
		foreach (var configurationSetting in settings)
		{
			foreach (var keyFilter in keyFiltersFromLongestPrefixToShortest)
			{
				var startsWithPrefix = configurationSetting.Key.StartsWith($"{keyFilter.Prefix}{ConfigurationPath.KeyDelimiter}");
				if (startsWithPrefix)
				{
					filteredConfigurationSettings.Add(
						keyFilter.AmendSettingKey(configurationSetting.Key),
						configurationSetting.Value);

					break;
				}
			}
		}

		return filteredConfigurationSettings;
	}
}
