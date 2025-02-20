using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.SwaggerGeneration;

class ConstrainedTypeTransparencySchemasFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (ConstrainedTypeInfos.TryGet(context.Type, out var constrainedTypeInfo))
		{
			var fullyNativeSchema = context.SchemaGenerator.GenerateSchema(constrainedTypeInfo.FullyNativeRootType, context.SchemaRepository);
			var bestExample = context.Type.IsOrImplementsGenericInterface(typeof(IConstrainedValue<,>))
				? GetBestValueExample(constrainedTypeInfo.ValidValueExamples)
				: GetBestCollectionExample(constrainedTypeInfo.ValidValueExamples);

			Overwrite(
				schema,
				with: fullyNativeSchema,
				example: bestExample);
		}
	}

	object GetBestValueExample(IList validValueExamples)
	=> validValueExamples[0] ?? validValueExamples[1]!;

	object GetBestCollectionExample(IList validValueExamples)
	{
		for (int i = 0; i < validValueExamples.Count; i++)
		{
			ICollection collectionExample = (ICollection)validValueExamples[i]!;
			if (collectionExample.Count > 1)
			{
				return collectionExample;
			}
		}
		for (int i = 0; i < validValueExamples.Count; i++)
		{
			ICollection collectionExample = (ICollection)validValueExamples[i]!;
			if (collectionExample.Count == 1)
			{
				return collectionExample;
			}
		}

		return validValueExamples[0] ?? validValueExamples[1]!;
	}

	public static void Overwrite(OpenApiSchema schema, OpenApiSchema with, object example)
	{
		schema.Title = with.Title;
		schema.Description = with.Description;
		schema.Format = with.Format;
		schema.Nullable = with.Nullable;
		schema.ReadOnly = with.ReadOnly;
		schema.WriteOnly = with.WriteOnly;
		schema.Deprecated = with.Deprecated;
		schema.UnresolvedReference = with.UnresolvedReference;

		schema.Default = with.Default;
		schema.Example = with.Example;
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

		schema.Reference = with.Reference;
		schema.Discriminator = with.Discriminator;

		schema.Example = OpenApiAnyFactory.CreateFromJson(example.GetStringRepresentation());
	}
}
