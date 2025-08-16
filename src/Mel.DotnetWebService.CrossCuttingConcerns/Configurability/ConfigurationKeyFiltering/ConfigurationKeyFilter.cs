using Microsoft.Extensions.Configuration;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;

public class ConfigurationKeyFilter
{
	public string Prefix { get; }
	public ConfigurationKeyFilterPrefixPresence PrefixPresencePolicy { get; }

	ConfigurationKeyFilter(string prefix, ConfigurationKeyFilterPrefixPresence prefixPresencePolicy)
	{
		Prefix = prefix switch
		{
			null => throw ObjectConstructionException.WhenConstructingAMemberFor<ConfigurationKeyFilter>(nameof(Prefix), prefix, "@member cannot but null"),
			"" => throw ObjectConstructionException.WhenConstructingAMemberFor<ConfigurationKeyFilter>(nameof(Prefix), prefix, "@member cannot but empty"),
			_ => prefix
		};

		PrefixPresencePolicy = prefixPresencePolicy switch
		{
			ConfigurationKeyFilterPrefixPresence.TechnicalDefaultEnumValue => throw ObjectConstructionException.WhenConstructingAMemberFor<ConfigurationKeyFilter>(nameof(PrefixPresencePolicy), prefixPresencePolicy, $"{nameof(PrefixPresencePolicy)}.{nameof(PrefixPresencePolicy.TechnicalDefaultEnumValue)}"),
			ConfigurationKeyFilterPrefixPresence.MustBeExplicit => prefixPresencePolicy,
			ConfigurationKeyFilterPrefixPresence.CanBeImplicit => prefixPresencePolicy,
			var unnamedEnumValue => throw ObjectConstructionException.WhenConstructingAMemberFor<ConfigurationKeyFilter>(nameof(PrefixPresencePolicy), prefixPresencePolicy, $"{nameof(PrefixPresencePolicy)}:{(int)unnamedEnumValue}")
		};
	}

	public static readonly ConfigurationKeyFilter ExplicitlyPrivateConfigurationSettings = new("private", ConfigurationKeyFilterPrefixPresence.MustBeExplicit);
	public static readonly ConfigurationKeyFilter ImplicitlyPrivateConfigurationSettings = new("private", ConfigurationKeyFilterPrefixPresence.CanBeImplicit);
	public static readonly ConfigurationKeyFilter ExplicitlyPublicConfigurationSettings = new("public", ConfigurationKeyFilterPrefixPresence.MustBeExplicit);
	public static readonly ConfigurationKeyFilter ImplicitlyPublicConfigurationSettings = new("public", ConfigurationKeyFilterPrefixPresence.CanBeImplicit);

	public string AmendSettingKey(string settingKey)
	{
		var prefixToRemove = $"{Prefix}:{ConfigurationPath.KeyDelimiter}";
		var settingKeyStartsWithPrefixToRemove = settingKey.StartsWith(prefixToRemove);

		return (PrefixPresencePolicy, settingKeyStartsWithPrefixToRemove) switch
		{
			(ConfigurationKeyFilterPrefixPresence.MustBeExplicit, true) => settingKey[prefixToRemove.Length..],
			(ConfigurationKeyFilterPrefixPresence.CanBeImplicit, true) => settingKey[prefixToRemove.Length..],
			(ConfigurationKeyFilterPrefixPresence.MustBeExplicit, false) => throw new InvalidOperationException($"{nameof(ConfigurationKeyFilter)} : Cannot {nameof(AmendSettingKey)} on {settingKey} : that key does not start with {prefixToRemove}."),
			(ConfigurationKeyFilterPrefixPresence.CanBeImplicit, false) => settingKey,
			_ => throw new NotImplementedException()
		};
	}
}
