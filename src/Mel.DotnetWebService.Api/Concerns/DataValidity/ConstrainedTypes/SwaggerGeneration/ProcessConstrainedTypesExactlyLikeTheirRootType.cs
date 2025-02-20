using Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.SwaggerGeneration;

class ProcessConstrainedTypesExactlyLikeTheirRootType :  IConfigureNamedOptions<SwaggerGenOptions>
{
	readonly RuntimeControllerActionsAnalyzer _runtimeControllerActionsAnalyzer;

	public ProcessConstrainedTypesExactlyLikeTheirRootType(RuntimeControllerActionsAnalyzer runtimeControllerActionsAnalyzer)
	{
		_runtimeControllerActionsAnalyzer = runtimeControllerActionsAnalyzer;
	}

	public void Configure(string? name, SwaggerGenOptions options)
	=> Configure(options);

	public void Configure(SwaggerGenOptions options)
	{
		foreach (var constrainedType in _runtimeControllerActionsAnalyzer.ConstrainedTypesInvolvedInControllerActionSignatures)
		{
			var rootType = constrainedType.GetConstrainedTypeRootType();
			if (rootType!.ImplementsGenericInterface(typeof(IEnumerable<>), out var argumentTypes))
			{
				if (argumentTypes.First().IsGenericTypeParameter)
				{
					continue;
				}
				else
				{
					options.MapType(constrainedType, () => new OpenApiSchema
					{
						Type = Integration.ConstrainedTypes.SwaggerGeneration.OpenApiDataType.Array,
						Items = Integration.ConstrainedTypes.SwaggerGeneration.PrimitiveTypesAndFormatsByDeclaringType[argumentTypes.First()!]
					});
				}
			}
			else
			{
				options.MapType(constrainedType, () => Integration.ConstrainedTypes.SwaggerGeneration.PrimitiveTypesAndFormatsByDeclaringType[rootType!]);
			}
		}
	}
}
