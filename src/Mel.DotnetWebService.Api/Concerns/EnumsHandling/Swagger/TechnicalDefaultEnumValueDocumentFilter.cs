using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling.Swagger;

class TechnicalDefaultEnumValueDocumentFilter : IDocumentFilter
{
	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		var schemasWithEnumType = swaggerDoc.Components.Schemas
			.Select(schema => schema.Value)
			.Where(enumDefinition => enumDefinition.Enum is [_, ..]);

		foreach (var schemaWithEnumType in schemasWithEnumType)
		{
			var enumStringValuesExceptTechnicalDefaultEnumValue = schemaWithEnumType.Enum
				.Where(iOpenApiAny => iOpenApiAny is not OpenApiString { Value: "TechnicalDefaultEnumValue" })
				.ToList();

			schemaWithEnumType.Enum = enumStringValuesExceptTechnicalDefaultEnumValue;
		}
	}
}
