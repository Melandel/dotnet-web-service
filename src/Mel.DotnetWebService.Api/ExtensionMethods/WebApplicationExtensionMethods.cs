namespace Mel.DotnetWebService.Api.ExtensionMethods;

public static class WebApplicationExtensionMethods
{
	public static WebApplication UseCustomSwaggerUI(this WebApplication app)
	{
		app.UseSwagger();
		app.UseSwaggerUI(swaggerUIOptions => swaggerUIOptions.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List));

		return app;
	}
}
