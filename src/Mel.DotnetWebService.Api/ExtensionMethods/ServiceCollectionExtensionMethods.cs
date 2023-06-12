namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ServiceCollectionExtensionMethods
{
	public static IServiceCollection AddCustomSwaggerGeneration(this IServiceCollection services)
	{
		services
			.AddSwaggerGen()
			.AddEndpointsApiExplorer()
			.ConfigureOptions<Concerns.SwaggerGeneration.WebServiceMetadataDocumentation.ProvideWebServiceWithTitleAndDescription>();

		return services;
	}

	public static IServiceCollection AddCustomSwaggerUI(this IServiceCollection services)
	{
		services
			.ConfigureOptions<Concerns.SwaggerUI.SetSwaggerUiRoutePrefix>()
			.ConfigureOptions<Concerns.SwaggerUI.Layout.CollapseSwaggerUiSectionsForReadability>();

		return services;
	}
}
