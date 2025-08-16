using Mel.DotnetWebService.CrossCuttingConcerns.Configurability.ConfigurationKeyFiltering;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability.JsonConfigurationFile;

class FilteredJsonConfigurationSource : JsonConfigurationSource
{
	public IReadOnlyCollection<ConfigurationKeyFilter> Filters { get; set; } = Array.Empty<ConfigurationKeyFilter>();
	public override IConfigurationProvider Build(IConfigurationBuilder builder)
	{
		EnsureDefaults(builder);

		return Filters.Any()
			? new FilteredJsonConfigurationProvider(this, NonEmptyHashSet<ConfigurationKeyFilter>.Storing(Filters))
			: new JsonConfigurationProvider(this);
	}
}
