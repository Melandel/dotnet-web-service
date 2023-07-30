using System.Text.Json.Serialization;
using Mel.DotnetWebService.Api.Controllers;
using Mel.DotnetWebService.Api.EnumsHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

public static class ServiceCollectionExtensionMethods
{
	public static IServiceCollection AddCustomSerializationSettings(this IServiceCollection services)
	{
		services.ConfigureOptions<ConfigureMvcJsonOptions>();

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
			.ConfigureOptions<ConfigureSwaggerOptions>();

		return services;
	}

	public static IServiceCollection AddCustomVersionedControllers(this IServiceCollection services)
	{
		services.AddControllers();
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
		services.AddHttpContextAccessor();
		services.AddScoped<HttpProblemTypeProvider>();
		return services;
	}

	public class ConfigureMvcJsonOptions : IConfigureNamedOptions<JsonOptions>
	{
		public void Configure(string? name, JsonOptions options) => Configure(options);
		public void Configure(JsonOptions options)
		{
			options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
		}
	}

	public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
	{
		const string WebApiTitle = "Dotnet Web Service";
		const string WebApiDescriptionSuffixWhenVersionIsDeprecated = "<i>This API version has been deprecated. Please use one of the new APIs available from the explorer.</i>";
		const string WebApiDescription = @"
<ul>
<li>The Dotnet Web Service API is a template API meant for sharing Minh-Tâm's vision of a clean web API.</li>
<li>The <b>main use case</b> is getting a ""Hello, SomeName."" message.</li>
<li><b>For a better introduction or more information, visit the <a href=""https://github.com/Melandel/team-wiki/wiki/%F0%9F%8E%A8-MyNiceProduct"">comprehensive documentation</a>.</b></li>
</ul>";

		readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
		{
			_apiVersionDescriptionProvider = apiVersionDescriptionProvider;
		}

		public void Configure(string? name, SwaggerGenOptions options) => Configure(options);
		public void Configure(SwaggerGenOptions options)
		{
			foreach (var apiVersionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
			{
				var openApiDocumentId = apiVersionDescription.GroupName;
				var openApiDocument = new OpenApiInfo
				{
					Title = WebApiTitle,
					Version = apiVersionDescription.ApiVersion.ToString(),
					Description = apiVersionDescription.IsDeprecated
						? WebApiDescription + WebApiDescriptionSuffixWhenVersionIsDeprecated
						: WebApiDescription
				};

				options.SwaggerDoc(openApiDocumentId, openApiDocument);
			}

			options.DocumentFilter<TechnicalDefaultEnumValueDocumentFilter>();
		}
	}
}

