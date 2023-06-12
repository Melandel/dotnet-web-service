using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.SwaggerGeneration.WebServiceMetadataDocumentation;

public class ProvideWebServiceWithTitleAndDescription : IConfigureNamedOptions<SwaggerGenOptions>
{
	public void Configure(string? name, SwaggerGenOptions options)
	=> Configure(options);

	public void Configure(SwaggerGenOptions options)
	{
		var openApiDocumentId = Integration.WebServiceMetadataDocumentation.Version;
		var openApiDocument = new OpenApiInfo
		{
			Title = Integration.WebServiceMetadataDocumentation.Title,
			Description = Integration.WebServiceMetadataDocumentation.Description
		};
		options.SwaggerDoc(openApiDocumentId, openApiDocument);
	}
}
