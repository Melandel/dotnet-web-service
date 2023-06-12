namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ServiceCollectionExtensionMethods
{
	public static IServiceCollection AddCustomControllersAndCustomApiVersioning(this IServiceCollection services)
	{
		services.AddControllers();
		services.ConfigureOptions<Concerns.Routing.RouteNamingConvention.UseKebabCaseAsRouteNamingConvention>();

		services
			.AddVersionedApiExplorer()
			.ConfigureOptions<Concerns.Versioning.Routing.UseApiVersionInUris>();

		services
			.AddApiVersioning()
			.ConfigureOptions<Concerns.Versioning.HttpResponseHeaders.WriteApiSupportedAndDeprecatedVersions>();

		return services;
	}

	public static IServiceCollection AddCustomSwaggerGeneration(this IServiceCollection services)
	{
		services
			.AddSwaggerGen()
			.AddEndpointsApiExplorer()
			.ConfigureOptions<Concerns.Versioning.SwaggerGeneration.CreateOneSwaggerForEachApiVersion>()
			.ConfigureOptions<Concerns.SwaggerGeneration.WebServiceMetadataDocumentation.ProvideWebServiceWithTitleAndDescription>();
		return services;
	}

	public static IServiceCollection AddCustomSwaggerUI(this IServiceCollection services)
	{
		services
			.ConfigureOptions<Concerns.SwaggerUI.SetSwaggerUiRoutePrefix>()
			.ConfigureOptions<Concerns.SwaggerUI.Layout.CollapseSwaggerUiSectionsForReadability>();

		services.ConfigureOptions<Concerns.Versioning.SwaggerUI.DisplayLatestApiVersionFirst>();

		return services;
	}
}

