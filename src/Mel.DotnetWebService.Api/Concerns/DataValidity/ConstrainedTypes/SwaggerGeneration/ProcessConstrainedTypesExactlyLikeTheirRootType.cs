using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.SwaggerGeneration;

class ProcessConstrainedTypesExactlyLikeTheirRootType :  IConfigureNamedOptions<SwaggerGenOptions>
{
	public void Configure(string? name, SwaggerGenOptions options)
	=> Configure(options);

	public void Configure(SwaggerGenOptions options)
	{
		options.SchemaFilter<ConstrainedTypeTransparencySchemasFilter>();
	}
}
