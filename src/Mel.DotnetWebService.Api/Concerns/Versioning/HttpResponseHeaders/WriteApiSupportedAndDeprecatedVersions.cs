using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.Api.Concerns.Versioning.HttpResponseHeaders;

public class WriteApiSupportedAndDeprecatedVersions
	: IConfigureNamedOptions<ApiVersioningOptions>
{
	public void Configure(string? name, ApiVersioningOptions options)
	=> Configure(options);

	public void Configure(ApiVersioningOptions options)
	{
		options.ReportApiVersions = true;
	}
}
