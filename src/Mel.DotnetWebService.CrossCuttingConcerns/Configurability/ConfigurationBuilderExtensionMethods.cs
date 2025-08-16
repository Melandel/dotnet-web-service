using Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;
using Mel.DotnetWebService.CrossCuttingConcerns.Configurability.JsonConfigurationFile;
using Microsoft.Extensions.Configuration;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability;

static class ConfigurationBuilderExtensionMethods
{
	public static IConfigurationBuilder AddFilteredJsonFile(this IConfigurationBuilder builder,
		string path,
		IReadOnlyCollection<ConfigurationKeyFilter> filters,
		bool removePrefixFromKey = true,
		bool optional = false,
		bool reloadOnChange = false)
	{
		builder.Add<FilteredJsonConfigurationSource>(configurationSource =>
		{
			configurationSource.Path = path;
			configurationSource.Filters = filters;
			configurationSource.Optional = optional;
			configurationSource.ReloadOnChange = reloadOnChange;
			configurationSource.ResolveFileProvider();
		});

		return builder;
	}
}
