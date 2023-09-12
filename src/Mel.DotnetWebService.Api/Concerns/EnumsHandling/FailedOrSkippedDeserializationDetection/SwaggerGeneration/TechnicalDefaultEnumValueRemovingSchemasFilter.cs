using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling.FailedOrSkippedDeserializationDetection.SwaggerGeneration;

class TechnicalDefaultEnumValueRemovingSchemasFilter : ISchemaFilter
{
	public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
	{
		if (schema is OpenApiSchema openApiSchema && schema.Enum is [_, ..])
		{
			openApiSchema.Enum = schema.Enum
				.Where(jsonNode => !jsonNode.GetValue<string>().Matches(Integration.FailedOrSkippedDeserializationDetection.TechnicalDefaultEnumValue.Name))
				.ToList();
		}
	}
}
