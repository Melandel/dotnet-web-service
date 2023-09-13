using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.Api.Concerns.Routing.RouteNamingConvention;

public class UseKebabCaseAsRouteNamingConvention : IConfigureNamedOptions<MvcOptions>
{
	public void Configure(string? name, MvcOptions options)
	=> Configure(options);

	public void Configure(MvcOptions options)
	{
		options.Conventions.Add(Integration.RouteNamingConvention.KebabCaseTransformerConvention);
	}
}
