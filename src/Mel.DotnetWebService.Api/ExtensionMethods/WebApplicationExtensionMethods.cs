namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class WebApplicationExtensionMethods
{
	public static WebApplication UseCustomRewriter(this WebApplication app)
	{
		app.UseRewriter(Concerns.SwaggerUI.Integration.Routing.RewriteCommonlyUsedDefaultApiRoutePatternsIntoSwaggerUiRoute);
		return app;
	}

	public static WebApplication UseCustomControllers(this WebApplication app)
	{
		app.UseMiddleware<Concerns.Routing.ErrorHandling.ReturnEmpty404OrEmpty405DependingOnEndpointResolutionOutcome>();
		app.MapControllers();

		return app;
	}
}
