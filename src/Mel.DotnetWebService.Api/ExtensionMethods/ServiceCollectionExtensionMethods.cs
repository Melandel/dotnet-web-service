namespace Mel.DotnetWebService.Api.ExtensionMethods;

public static class ServiceCollectionExtensionMethods
{
	const string WebApiTitle = "Dotnet Web Service";
	const string WebApiDescription = @"
<ul>
<li>The Dotnet Web Service API is a template API meant for sharing Minh-Tâm's vision of a clean web API.</li>
<li>The <b>main use case</b> is getting a ""Hello, SomeName."" message.</li>
<li><b>For a better introduction or more information, visit the <a href=""https://github.com/Melandel/team-wiki/wiki/%F0%9F%8E%A8-MyNiceProduct"">comprehensive documentation</a>.</b></li>
</ul>";

	public static IServiceCollection AddCustomSwaggerGenerator(this IServiceCollection services)
	{
		var openApiDocumentId = "v1";
		var openApiDocument = new Microsoft.OpenApi.Models.OpenApiInfo
		{
			Title = WebApiTitle,
			Description = WebApiDescription
		};

		return services
			.AddEndpointsApiExplorer()
			.AddSwaggerGen(swaggerGenOptions => swaggerGenOptions.SwaggerDoc(openApiDocumentId, openApiDocument));
	}
}
