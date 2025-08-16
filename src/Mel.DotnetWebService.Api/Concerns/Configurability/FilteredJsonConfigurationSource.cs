using Microsoft.Extensions.Configuration.Json;

namespace Mel.DotnetWebService.Api.Concerns.Configurability;

class FilteredJsonConfigurationSource : JsonConfigurationSource
{
	public IReadOnlyCollection<JsonConfigurationFilter> Filters { get; set; } = Array.Empty<JsonConfigurationFilter>();
	public override IConfigurationProvider Build(IConfigurationBuilder builder)
	{
		EnsureDefaults(builder);

		if (!Filters.Any())
		{
			return new JsonConfigurationProvider(this);
		}

		return new FilteredJsonConfigurationProvider(this, Filters);
	}
}
