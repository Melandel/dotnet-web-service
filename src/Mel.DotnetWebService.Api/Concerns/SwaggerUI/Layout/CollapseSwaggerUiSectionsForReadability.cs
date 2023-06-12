using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Mel.DotnetWebService.Api.Concerns.SwaggerUI.Layout;

public class CollapseSwaggerUiSectionsForReadability : IConfigureNamedOptions<SwaggerUIOptions>
{
	public void Configure(string? name, SwaggerUIOptions options)
	=> Configure(options);

	public void Configure(SwaggerUIOptions options)
	{
		options.DocExpansion(DocExpansion.List);
	}
}
