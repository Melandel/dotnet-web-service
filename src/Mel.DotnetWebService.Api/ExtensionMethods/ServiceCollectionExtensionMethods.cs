using Mel.DotnetWebService.Api.Concerns.Routing.AttributeRouteTokenReplacement;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ServiceCollectionExtensionMethods
{
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
		services.AddControllers(mvcOptions =>
		{
			mvcOptions.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer()));
		});
		services.AddApiVersioning(apiVersioningOptions => apiVersioningOptions.ReportApiVersions = true);
		return services;
	}
}

