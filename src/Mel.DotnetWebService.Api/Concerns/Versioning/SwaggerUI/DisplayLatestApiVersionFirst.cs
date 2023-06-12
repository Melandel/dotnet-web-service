using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Mel.DotnetWebService.Api.Concerns.Versioning.SwaggerUI;

public class DisplayLatestApiVersionFirst : IConfigureNamedOptions<SwaggerUIOptions>
{
	readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
	public DisplayLatestApiVersionFirst(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
	{
		_apiVersionDescriptionProvider = apiVersionDescriptionProvider;
	}

	public void Configure(string? name, SwaggerUIOptions options)
	=> Configure(options);

	public void Configure(SwaggerUIOptions options)
	{
		var apiVersionDescriptionsFromMostRecentToOldest = _apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse();
		foreach (var apiVersionDescription in apiVersionDescriptionsFromMostRecentToOldest)
		{
			options.SwaggerEndpoint(
				String.Format("/{0}/{1}/swagger.json",
					Concerns.SwaggerUI.Integration.AccessToSwaggerUI.RoutePrefix,
					apiVersionDescription.GroupName),
				apiVersionDescription.GroupName);
		}
	}
}
