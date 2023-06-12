using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.Versioning.SwaggerGeneration;
class CreateOneSwaggerForEachApiVersion : IConfigureNamedOptions<SwaggerGenOptions>
{
	readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
	public CreateOneSwaggerForEachApiVersion(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
	{
		_apiVersionDescriptionProvider = apiVersionDescriptionProvider;
	}

	public void Configure(string? name, SwaggerGenOptions options) => Configure(options);
	public void Configure(SwaggerGenOptions options)
	{
		foreach (var apiVersionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
		{
			var openApiDocumentId = apiVersionDescription.GroupName;
			var openApiDocument = new OpenApiInfo
			{
				Version = apiVersionDescription.ApiVersion.ToString()
			};

			if (apiVersionDescription.IsDeprecated)
			{
				openApiDocument.Description = Integration.SwaggerGeneration.WebServiceDescriptionAddendumWhenVersionIsDeprecated;
			}

			options.SwaggerDoc(openApiDocumentId, openApiDocument);
		}
	}
}
