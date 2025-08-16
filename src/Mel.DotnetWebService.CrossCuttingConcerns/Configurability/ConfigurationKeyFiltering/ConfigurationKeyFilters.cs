using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;

public static class ConfigurationKeyFilters
{
	public static readonly NonEmptyHashSet<ConfigurationKeyFilter> ExplicitPublicAndImplicitPrivate = NonEmptyHashSet.Storing(
		ConfigurationKeyFilter.ExplicitlyPublicConfigurationSettings,
		ConfigurationKeyFilter.ImplicitlyPrivateConfigurationSettings
	);
}
