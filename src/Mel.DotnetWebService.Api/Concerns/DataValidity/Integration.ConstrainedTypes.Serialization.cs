using System.Text;
using Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity;

static partial class Integration
{
	public static partial class ConstrainedTypes
	{
		public class Serialization
		{
			public static void EnsureThatConstrainedTypesInvolvedInControllerActionSignaturesCanBeDeserialized(Concerns.RuntimeValidation.RuntimeValidator runtimeValidator)
			{
				var analyzer = new RuntimeControllerActionsAnalyzer(runtimeValidator.ControllerActionsExplorer);
				var constrainedTypesThatDontDefineHowToBeSerialized = analyzer.ConstrainedTypesInvolvedInControllerActionSignaturesThatDontDefineHowToBeDeserialized;
				if (constrainedTypesThatDontDefineHowToBeSerialized.Any())
				{
					throw new NotImplementedException(BuildErrorMessage(constrainedTypesThatDontDefineHowToBeSerialized));
				}
			}

			static string BuildErrorMessage(HashSet<Type> constrainedTypesThatDontDefineHowToBeSerialized)
			{
				var constrainedTypeName = typeof(Constrained<>).GetName();
				var constrainedGenericRootTypeName = constrainedTypeName[(constrainedTypeName.IndexOf('<') + 1)..constrainedTypeName.IndexOf('>')];
				var iCanBeDeserializedFromJsonTypeName = $"{typeof(ICanBeDeserializedFromJson<,>).Name[..^"`2".Length]}<TTypeInsideJson*,{constrainedGenericRootTypeName}>";

				var message = new StringBuilder($"The following {constrainedTypeName}-extending types are involved in {Concerns.SwaggerGeneration.Integration.WebServiceMetadataDocumentation.Title}'s API")
					.AppendLine($" and thus, must implement {iCanBeDeserializedFromJsonTypeName} for the web service to be able to read http requests :")
					.AppendLine()
					.AppendLine(string.Join(',', constrainedTypesThatDontDefineHowToBeSerialized.Select(t => t.FullName)))
					.AppendLine()
					.Append($"* with TTypeInsideJson one of the following : {string.Join(',', Utf8JsonReaderExtensionMethods.NativelySupportedTypes.Select(t => t.Name))})")
					.ToString();

				return message;
			}
		}
	}
}
