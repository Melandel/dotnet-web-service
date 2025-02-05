using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.HttpProblemTypes;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling.IntegerToEnumTypeProhibition.ErrorHandling.ExceptionToHttpProblemConversion;

static class EnumValueReceivedFromIntegerExceptionConversion
{
	public static HttpProblem ConvertEnumValueReceivedFromIntegerException(
		EnumValueReceivedFromIntegerException ex,
		Uri someFormOfIdentifierForTheProblemOccurrence,
		HttpProblemTypeProvider httpProblemTypeProvider)
	=> HttpProblem.From(
		httpProblemTypeProvider.GetEnumValueReceivedFromInteger(),
		HttpProblemOccurrence.FromIncorrectApiUsage(
			id: someFormOfIdentifierForTheProblemOccurrence,
			statementDescribingIncorrectApiUsage: $"Parameter \"{ex.ParameterName}\" does not accept value {ex.IntegerValue}.",
			statementHelpingApiUserTowardsCorrectApiUsage: $"Supported values: {string.Join(", ", ex.SupportedEnumValues)}.",
			problemSpecificInformation: new Dictionary<HttpProblemTypeExtensionMember, object>
			{
		{ HttpProblemTypeExtensionMember.IntegerValue, ex.IntegerValue },
		{ HttpProblemTypeExtensionMember.EnumParameterName, ex.ParameterName },
		{ HttpProblemTypeExtensionMember.SupportedEnumValues, ex.SupportedEnumValues }
	}));
}
