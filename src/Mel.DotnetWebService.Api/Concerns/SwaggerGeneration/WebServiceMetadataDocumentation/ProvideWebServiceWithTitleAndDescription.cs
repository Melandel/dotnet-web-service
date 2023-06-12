using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.SwaggerGeneration.WebServiceMetadataDocumentation;

public class ProvideWebServiceWithTitleAndDescription : IConfigureNamedOptions<SwaggerGenOptions>
{
	public void Configure(string? name, SwaggerGenOptions options)
	=> Configure(options);

	public void Configure(SwaggerGenOptions options)
	{
		foreach (var openApiDocument in options.SwaggerGeneratorOptions.SwaggerDocs.Values)
		{
			openApiDocument.Title = Integration.WebServiceMetadataDocumentation.Title;
			openApiDocument.Description = Integration.WebServiceMetadataDocumentation.Description + Environment.NewLine + openApiDocument.Description;
		}
	}
}
