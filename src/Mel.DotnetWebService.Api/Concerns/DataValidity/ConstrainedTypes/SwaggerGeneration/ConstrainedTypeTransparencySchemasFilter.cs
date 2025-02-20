using System.Collections;
using System.Text.Json.Nodes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.SwaggerGeneration;

class ConstrainedTypeTransparencySchemasFilter : ISchemaFilter
{
	public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
	{
		if (schema is OpenApiSchema openApiSchema && ConstrainedTypeInfos.TryGet(context.Type, out var constrainedTypeInfo))
		{
			var fullyNativeSchema = context.SchemaGenerator.GenerateSchema(constrainedTypeInfo.FullyNativeRootType, context.SchemaRepository);

			Overwrite(
				openApiSchema,
				with: fullyNativeSchema,
				examples: constrainedTypeInfo.ValidValueExamples);
		}
	}

	public static void Overwrite(OpenApiSchema schema, IOpenApiSchema with, IList examples)
	{
		OverwriteNullableType(schema, with);

		schema.Type = with.Type;
		schema.Title = with.Title;
		schema.Description = with.Description;
		schema.Format = with.Format;
		schema.ReadOnly = with.ReadOnly;
		schema.WriteOnly = with.WriteOnly;
		schema.Deprecated = with.Deprecated;

		schema.Default = with.Default;
		schema.Enum = with.Enum;
		schema.Required = with.Required;

		schema.Properties = with.Properties;
		schema.Items = with.Items;
		schema.AdditionalProperties = with.AdditionalProperties;
		schema.AdditionalPropertiesAllowed = with.AdditionalPropertiesAllowed;

		schema.AllOf = with.AllOf;
		schema.AnyOf = with.AnyOf;
		schema.OneOf = with.OneOf;
		schema.Not = with.Not;

		schema.Minimum = with.Minimum;
		schema.Maximum = with.Maximum;
		schema.ExclusiveMinimum = with.ExclusiveMinimum;
		schema.ExclusiveMaximum = with.ExclusiveMaximum;
		schema.MultipleOf = with.MultipleOf;

		schema.MinLength = with.MinLength;
		schema.MaxLength = with.MaxLength;
		schema.Pattern = with.Pattern;

		schema.MinItems = with.MinItems;
		schema.MaxItems = with.MaxItems;
		schema.UniqueItems = with.UniqueItems;

		schema.MinProperties = with.MinProperties;
		schema.MaxProperties = with.MaxProperties;

		schema.ExternalDocs = with.ExternalDocs;
		schema.Xml = with.Xml;
		schema.Extensions = with.Extensions;

		schema.Discriminator = with.Discriminator;

		OverwriteExamples(schema, examples);
	}

	static void OverwriteExamples(OpenApiSchema schema, IList examples)
	{
		var jsonNodeExamples = new JsonNode[examples.Count];
		for (var i = 0; i < examples.Count; i++)
		{
			jsonNodeExamples[i] = JsonNode.Parse(examples[i].GetStringRepresentation())!;
		}
		schema.Examples = jsonNodeExamples;
	}

	static void OverwriteNullableType(OpenApiSchema schema, IOpenApiSchema with)
	{
		bool withIsNullable = with.Type?.HasFlag(JsonSchemaType.Null) == true;
		bool schemaIsNullable = schema.Type?.HasFlag(JsonSchemaType.Null) == true;
		if (!schemaIsNullable && withIsNullable)
		{
			schema.Type |= JsonSchemaType.Null;
		}
		else if (schemaIsNullable && !withIsNullable)
		{
			schema.Type &= ~JsonSchemaType.Null;
		}
	}
}
