using Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
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
			var rootType = ConstrainedTypeInfos.GetRootType(constrainedType);
			if (rootType.ImplementsGenericInterface(typeof(IEnumerable<>), out var argumentTypes))
			{
				var elementType = argumentTypes.First();
				if (elementType.IsGenericTypeParameter)
				{
					continue;
				}
				else
				{
					var elementRootType = elementType switch
					{
						var t when ConstrainedTypeInfos.TryGet(t, out var constrainedElementTypeInfo) => constrainedElementTypeInfo.RootType,
						var t => t
					};
					options.MapType(constrainedType, () => new OpenApiSchema
					{
						Type = Integration.ConstrainedTypes.SwaggerGeneration.OpenApiDataType.Array,
						Items = Integration.ConstrainedTypes.SwaggerGeneration.PrimitiveTypesAndFormatsByDeclaringType[elementRootType!],
					});
				}
			}
			else
			{
				options.MapType(constrainedType, () =>
				{
					var schema = Integration.ConstrainedTypes.SwaggerGeneration.PrimitiveTypesAndFormatsByDeclaringType[rootType];
					if (ConstrainedTypeInfos.TryGet(constrainedType, out var constrainedTypeInfo))
					{
						schema.Example = Integration.ConstrainedTypes.SwaggerGeneration.OpenApiPrimitiveBuilder.BuildFrom(constrainedTypeInfo.ValidValueExamples.First());
					}
					return schema;
				});
			}
		}
	}
}
