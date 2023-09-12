using System.Text;
using Mel.DotnetWebService.Api.Concerns.EnumsHandling.FailedOrSkippedDeserializationDetection.Serialization;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling;

static partial class Integration
{
	public partial class FailedOrSkippedDeserializationDetection
	{
		public class Serialization
		{
			public static void EnsureThatEnumTypesInvolvedInsideControllerActionSignaturesCanBeDetectFailedOrSkippedDeserialization(Concerns.RuntimeValidation.RuntimeValidator runtimeValidator)
			{
				var analyzer = new RuntimeControllerActionsAnalyzer(runtimeValidator.ControllerActionsExplorer);
				var enumTypesThatCannotBeDetectedWhenDeserializationFailsOrIsSkipped = analyzer.EnumTypesInvolvedInControllerActionSignaturesThatDontDefineTechnicalDefaultEnumValue;
				if (enumTypesThatCannotBeDetectedWhenDeserializationFailsOrIsSkipped.Any())
				{
					throw new NotImplementedException(BuildErrorMessage(enumTypesThatCannotBeDetectedWhenDeserializationFailsOrIsSkipped));
				}
			}
			static string BuildErrorMessage(HashSet<Type> enumTypesThatCannotBeDetectedWhenDeserializationFailsOrIsSkipped)
			{
				var message = new StringBuilder($"The following {nameof(Enum)} types are involved in {SwaggerGeneration.Integration.WebServiceMetadataDocumentation.Title}'s API")
					.AppendLine($" and thus, must define the value {{{TechnicalDefaultEnumValue.Value}, {TechnicalDefaultEnumValue.Name}}} for the web service to be able to detect whether the enum value is meaningful, or the consequence of deserialization failure/skipping")
					.AppendLine()
					.Append(string.Join(',', enumTypesThatCannotBeDetectedWhenDeserializationFailsOrIsSkipped.Select(t => t.FullName)))
					.ToString();

				return message;
			}
		}
	}
}
