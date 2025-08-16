using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.HttpProblemTypes;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ServiceCollectionExtensionMethods
{
	public static IServiceCollection AddCustomControllersAndCustomApiVersioning(this IServiceCollection services)
	{
		services.AddControllers();
		services
			.ConfigureOptions<Concerns.Routing.RouteNamingConvention.UseKebabCaseAsRouteNamingConvention>()
			.ConfigureOptions<Concerns.DataValidity.ConstrainedTypes.ModelBinding.ProcessConstrainedTypesExactlyLikeTheirRootType>()
			.ConfigureOptions<Concerns.DataValidity.ConstrainedTypes.Serialization.ProcessConstrainedTypesExactlyLikeTheirRootType>();
		services
			.AddVersionedApiExplorer()
			.ConfigureOptions<Concerns.Versioning.Routing.UseApiVersionInUris>();

		services
			.AddApiVersioning()
			.ConfigureOptions<Concerns.Versioning.HttpResponseHeaders.WriteApiSupportedAndDeprecatedVersions>();

		return services;
	}

	public static IServiceCollection AddCustomSerializationSettings(this IServiceCollection services)
	{
		services.ConfigureOptions<Concerns.EnumsHandling.IntegerToEnumTypeProhibition.Serialization.ProhibitIntegerToEnumTypeSerializationAttribute>();
		return services;
	}

	public static IServiceCollection AddCustomSwaggerGeneration(this IServiceCollection services)
	{
		services
			.AddSwaggerGen()
			.AddEndpointsApiExplorer()
			.ConfigureOptions<Concerns.Versioning.SwaggerGeneration.CreateOneSwaggerForEachApiVersion>()
			.ConfigureOptions<Concerns.SwaggerGeneration.WebServiceMetadataDocumentation.ProvideWebServiceWithTitleAndDescription>()
			.AddTransient<Concerns.DataValidity.ConstrainedTypes.Serialization.RuntimeControllerActionsAnalyzer>()
			.ConfigureOptions<Concerns.DataValidity.ConstrainedTypes.SwaggerGeneration.ProcessConstrainedTypesExactlyLikeTheirRootType>()
			.ConfigureOptions<Concerns.EnumsHandling.FailedOrSkippedDeserializationDetection.SwaggerGeneration.RemoveTechnicalDefaultEnumValueFromDocumentedEnumValues>();
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

	public static IServiceCollection AddCustomExceptionHandlingCompliantWithRfc9457(this IServiceCollection services)
	{
		services.AddProblemDetails();
		services.AddAccessToHttpProblemTypeProviderFromReceivingControllerAndErrorHandlingMiddleware();
		return services;
	}

	public static IServiceCollection AddAccessToHttpProblemTypeProviderFromReceivingControllerAndErrorHandlingMiddleware(this IServiceCollection services)
	{
		services.AddScoped<HttpProblemTypeProvider>();
		services.AddHttpContextAccessor();
		return services;
	}

	public static IServiceCollection AddCustomRuntimeValidation(this IServiceCollection services)
	{
		services.AddSingleton<Concerns.RuntimeValidation.ControllerActionsExplorer>();
		services.AddSingleton<Concerns.RuntimeValidation.RuntimeValidator>();
		return services;
	}
}
