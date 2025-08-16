using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;

public static class ConfigurationKeyFilters
{
	public static readonly NonEmptyHashSet<ConfigurationKeyFilter> ExplicitPublicAndImplicitPrivate = NonEmptyHashSet.CreateFromElements(
		ConfigurationKeyFilter.ExplicitlyPublicConfigurationSettings,
		ConfigurationKeyFilter.ImplicitlyPrivateConfigurationSettings
	);
}
