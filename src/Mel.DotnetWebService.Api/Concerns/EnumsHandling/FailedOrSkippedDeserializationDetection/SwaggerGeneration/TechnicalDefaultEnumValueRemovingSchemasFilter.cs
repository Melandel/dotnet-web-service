using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling.FailedOrSkippedDeserializationDetection.SwaggerGeneration;

class TechnicalDefaultEnumValueRemovingSchemasFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (schema.Enum is [_, ..])
		{
			schema.Enum = schema.Enum
				.Where(iOpenApiAny => iOpenApiAny is not OpenApiString { Value: Integration.FailedOrSkippedDeserializationDetection.TechnicalDefaultEnumValue.Name })
				.ToList();
		}
	}
}
