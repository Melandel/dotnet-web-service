using Microsoft.AspNetCore.Rewrite;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

public static class WebApplicationExtensionMethods
{
	const string SwaggerRoutePrefix = "swagger";
	const string SwaggerRoute = $"{SwaggerRoutePrefix}/index.html";
	public static WebApplication UseCustomSwaggerUI(this WebApplication app)
	{
		app.UseSwagger();

		app.UseSwaggerUI(swaggerUIOptions =>
		{
			swaggerUIOptions.RoutePrefix = SwaggerRoutePrefix;
			swaggerUIOptions.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
		});

		app.UseCustomRedirectionToSwaggerUiOnCommonlyUsedDefaultApiRoutes();

		return app;
	}

	public static WebApplication UseCustomRedirectionToSwaggerUiOnCommonlyUsedDefaultApiRoutes(this WebApplication app)
	{
		var rewriteOptions = new RewriteOptions()
			.AddRedirect("^$", SwaggerRoute)
			.AddRedirect("^home.html$", SwaggerRoute)
			.AddRedirect("^index.html$", SwaggerRoute);

		app.UseRewriter(rewriteOptions);

		return app;
	}
}
