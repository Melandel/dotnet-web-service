namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class WebApplicationExtensionMethods
{
	public static WebApplication UseCustomRewriter(this WebApplication app)
	{
		app.UseRewriter(Concerns.SwaggerUI.Integration.Routing.RewriteCommonlyUsedDefaultApiRoutePatternsIntoSwaggerUiRoute);
		return app;
	}
}
