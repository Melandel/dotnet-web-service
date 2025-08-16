using Mel.DotnetWebService.Api.Concerns.Routing.AttributeRouteTokenReplacement;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ServiceCollectionExtensionMethods
{
	public static IServiceCollection AddCustomSerializationSettings(this IServiceCollection services)
	{
		services.ConfigureOptions<Concerns.EnumsHandling.Serialization.ConfigureMvcJsonOptions>();

		return services;
	}

	public static IServiceCollection AddCustomSwaggerGenerator(this IServiceCollection services)
	{
		services
			.AddSwaggerGen()
			.AddVersionedApiExplorer(apiExplorerOptions =>
			{
				apiExplorerOptions.GroupNameFormat = "'v'V";
				apiExplorerOptions.SubstituteApiVersionInUrl = true;
			})
			.ConfigureOptions<Concerns.SwaggerUI.ConfigureSwaggerOptions>();

		return services;
	}

	public static IServiceCollection AddCustomControllers(this IServiceCollection services)
	{
		services
			.AddControllers(mvcOptions =>
			{
				mvcOptions.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer()));
				mvcOptions.ModelBinderProviders.Insert(0, new NonEmptyGuidBinderProvider());
			})
			.AddJsonOptions(jsonOptions =>
			{
				jsonOptions.JsonSerializerOptions.Converters.Add(new ConstrainedTypeJsonConverter());
			});
		services.AddApiVersioning(apiVersioningOptions => apiVersioningOptions.ReportApiVersions = true);
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
		services.AddScoped<Concerns.ErrorHandling.HttpProblemTypeProvider>();
		services.AddHttpContextAccessor();
		return services;
	}
}
