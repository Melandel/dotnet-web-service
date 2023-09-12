using Mel.DotnetWebService.Api.Concerns.RuntimeValidation;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;
using static Mel.DotnetWebService.Api.Concerns.EnumsHandling.Integration.FailedOrSkippedDeserializationDetection;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling.FailedOrSkippedDeserializationDetection.Serialization;

class RuntimeControllerActionsAnalyzer
{
	readonly ControllerActionsExplorer _controllerActionsExplorer;
	public RuntimeControllerActionsAnalyzer(ControllerActionsExplorer controllerActionsExplorer)
	{
		_controllerActionsExplorer = controllerActionsExplorer;
	}

	public HashSet<Type> EnumTypesInvolvedInControllerActionSignatures
	=> _controllerActionsExplorer.TypesInvolvedInControllerActionSignatures.Where(t => t.IsEnum).ToHashSet();

	public HashSet<Type> EnumTypesInvolvedInControllerActionSignaturesThatDontDefineTechnicalDefaultEnumValue
	=> EnumTypesInvolvedInControllerActionSignatures
		.Where(enumType => enumType.IsDefinedByOurOrganization())
		.Where(selfMadeEnumType =>
		{
			var enumDictionary = Enum.GetValues(selfMadeEnumType)
				.Cast<int>()
				.ToDictionary(enumValue => enumValue, enumValue => Enum.GetName(selfMadeEnumType, enumValue));

			var correctlyDefinesHowToBeDeserialized = enumDictionary.ContainsKey(TechnicalDefaultEnumValue.Value)
				&& enumDictionary[TechnicalDefaultEnumValue.Value] == TechnicalDefaultEnumValue.Name;

			return !correctlyDefinesHowToBeDeserialized;
		})
		.ToHashSet();
}
