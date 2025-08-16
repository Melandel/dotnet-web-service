using Mel.DotnetWebService.Api.Concerns.Configurability;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

public static partial class ConfigurationBuilderExtensions
{
	public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder,
		string path,
		IReadOnlyCollection<JsonConfigurationFilter> filters,
		bool removePrefixFromKey = true,
		bool optional = false,
		bool reloadOnChange = false)
	=> builder.Add<FilteredJsonConfigurationSource>(configurationSource =>
	{
		configurationSource.Path = path;
		configurationSource.Filters = filters;
		configurationSource.Optional = optional;
		configurationSource.ReloadOnChange = reloadOnChange;
		configurationSource.ResolveFileProvider();
	});
}
