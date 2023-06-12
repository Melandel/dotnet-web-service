using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.Api.Concerns.Versioning.Routing;

public class UseApiVersionInUris : IConfigureNamedOptions<ApiExplorerOptions>
{
	public void Configure(string? name, ApiExplorerOptions options)
	=> Configure(options);

	public void Configure(ApiExplorerOptions options)
	{
		options.GroupNameFormat = Integration.Routing.ApiVersionFormatInsideUri;
		options.SubstituteApiVersionInUrl = true;
	}
}
