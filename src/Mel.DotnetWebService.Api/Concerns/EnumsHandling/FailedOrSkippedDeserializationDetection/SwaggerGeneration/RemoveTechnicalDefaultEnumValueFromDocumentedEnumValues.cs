using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling.FailedOrSkippedDeserializationDetection.SwaggerGeneration;

class RemoveTechnicalDefaultEnumValueFromDocumentedEnumValues : IConfigureNamedOptions<SwaggerGenOptions>
{
	public void Configure(string? name, SwaggerGenOptions options)
	=> Configure(options);

	public void Configure(SwaggerGenOptions options)
	{
		options.DocumentFilter<TechnicalDefaultEnumValueDocumentFilter>();
	}
}
