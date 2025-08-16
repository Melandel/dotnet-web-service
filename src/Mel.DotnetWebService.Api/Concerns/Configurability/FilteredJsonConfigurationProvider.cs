using Microsoft.Extensions.Configuration.Json;

namespace Mel.DotnetWebService.Api.Concerns.Configurability;

class FilteredJsonConfigurationProvider : JsonConfigurationProvider
{
	readonly IReadOnlyCollection<JsonConfigurationFilter> _filters;
	public FilteredJsonConfigurationProvider(FilteredJsonConfigurationSource source, IReadOnlyCollection<JsonConfigurationFilter> filters) : base(source)
	{
		_filters = filters;
	}

	public override void Load(Stream stream)
	{
		base.Load(stream);

		var filteredConfigurationSettings = new Dictionary<string, string?>();
		foreach (var configurationSetting in Data)
		{
			foreach (var filter in _filters)
			{
				var startsWithPrefix = configurationSetting.Key.StartsWith(filter.Prefix);
				if (!startsWithPrefix)
				{
					continue;
				}

				filteredConfigurationSettings.Add(
					AmendConfigurationSettingKey(configurationSetting.Key, filter),
					configurationSetting.Value);

				break;
			}
		}

		Data = filteredConfigurationSettings;
	}

	string AmendConfigurationSettingKey(string configurationSettingKey, JsonConfigurationFilter filter)
	=> (configurationSettingKey, filter) switch
	{
		(_, { RemovePrefixFromConfigurationItemKey: false }) => configurationSettingKey,
		(_, { Prefix: var prefix }) when string.IsNullOrWhiteSpace(prefix) => configurationSettingKey,
		(var key, _) when !key.Contains(ConfigurationPath.KeyDelimiter) => key,
		(var key, { Prefix: { Length: var prefixLength } }) => key[(prefixLength + ConfigurationPath.KeyDelimiter.Length)..]
	};
}
