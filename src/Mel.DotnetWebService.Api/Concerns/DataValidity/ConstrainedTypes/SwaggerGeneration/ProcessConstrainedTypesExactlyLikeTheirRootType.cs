using Mel.DotnetWebService.Api.Concerns.RuntimeAnalysis;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.SwaggerGeneration;

class ProcessConstrainedTypesExactlyLikeTheirRootType :  IConfigureNamedOptions<SwaggerGenOptions>
{
	readonly ControllerActionsExplorer _controllerActionsExplorer;
	HashSet<ConstrainedTypeInfo> ConstrainedValueTypeInfosInvolvedInControllerActionSignatures
	=> _controllerActionsExplorer
		.TypesInvolvedInControllerActionSignatures
		.Where(t => t.IsOrImplementsInterface(typeof(IConstrainedValue<,>)))
		.Select(t => ConstrainedTypeInfos.Get(t))
		.ToHashSet();

	public ProcessConstrainedTypesExactlyLikeTheirRootType(ControllerActionsExplorer controllerActionsExplorer)
	{
		_controllerActionsExplorer = controllerActionsExplorer;
	}

	public void Configure(string? name, SwaggerGenOptions options)
	=> Configure(options);

	public void Configure(SwaggerGenOptions options)
	{
		foreach (var constrainedValueTypeInfo in ConstrainedValueTypeInfosInvolvedInControllerActionSignatures)
		{
			options.MapType(constrainedValueTypeInfo.Type, () =>
			{
				var schema = Integration.ConstrainedTypes.SwaggerGeneration.OpenApiSchemaBuilder.BuildFrom(constrainedValueTypeInfo.RootType);

				var fullyNativeExampleValues = constrainedValueTypeInfo.ValidValueExamples;
				schema.Example = Integration.ConstrainedTypes.SwaggerGeneration.OpenApiPrimitiveBuilder.BuildFrom(fullyNativeExampleValues[0] ?? fullyNativeExampleValues[1]!);

				return schema;
			});
		}
	}
}
