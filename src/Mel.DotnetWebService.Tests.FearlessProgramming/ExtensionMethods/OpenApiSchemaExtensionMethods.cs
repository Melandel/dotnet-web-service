using Microsoft.OpenApi;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

static class OpenApiSchemaExtensionMethods
{
	public static T GetFirstExample<T>(this OpenApiSchema schema)
	=>  schema.Examples[0].GetValue<T>();

	public static string[] GetEnumStringValues(this OpenApiSchema schema)
	=> schema.Enum.Select(jsonNode => jsonNode.GetValue<string>()).ToArray();
}
