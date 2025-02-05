using Mel.DotnetWebService.Api.Concerns.EnumsHandling.IntegerToEnumTypeProhibition.ErrorHandling.ExceptionToHttpProblemConversion;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.HttpProblemTypes;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling;

static partial class Integration
{
	public static class ErrorHandling
	{
		public static HttpProblem Convert(EnumValueReceivedFromIntegerException ex, Uri someFormOfIdentifierForTheProblemOccurrence, HttpProblemTypeProvider httpProblemTypeProvider)
		=> EnumValueReceivedFromIntegerExceptionConversion.ConvertEnumValueReceivedFromIntegerException(ex, someFormOfIdentifierForTheProblemOccurrence, httpProblemTypeProvider);
	}
}
