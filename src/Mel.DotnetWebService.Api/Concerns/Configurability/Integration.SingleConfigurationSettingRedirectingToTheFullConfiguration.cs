namespace Mel.DotnetWebService.Api.Concerns.Configurability;

static partial class Integration
{
	public static class SingleConfigurationSettingRedirectingToTheFullConfiguration
	{
		public const string ConfigurationLocationMainIdentifier = nameof(ConfigurationLocationMainIdentifier);
		public const string ConfigurationLocationFallbackIdentifier = nameof(ConfigurationLocationFallbackIdentifier);
	}
}
