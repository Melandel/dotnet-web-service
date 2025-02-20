using Microsoft.OpenApi;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

static class OpenApiDocumentExtensionMethods
{
	public static OpenApiSchema GetSchemaReferencedBy(this OpenApiDocument document, IOpenApiSchema schemaConstainingReference)
	=> schemaConstainingReference is OpenApiSchemaReference schemaReference
		? document.Components.Schemas[schemaReference.Reference.Id] as OpenApiSchema
		: schemaConstainingReference as OpenApiSchema;
}
