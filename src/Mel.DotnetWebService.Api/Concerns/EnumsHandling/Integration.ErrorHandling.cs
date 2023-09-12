using Mel.DotnetWebService.Api.Concerns.EnumsHandling.IntegerToEnumTypeProhibition.ErrorHandling.ExceptionToHttpProblemConversion;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.Concerns.EnumsHandling;

static partial class Integration
{
	public static class ErrorHandling
	{
		public static HttpProblem Convert(EnumValueReceivedFromIntegerException ex, Uri someFormOfIdentifierForTheProblemOccurrence, Controllers.HttpProblemTypeProvider httpProblemTypeProvider)
		=> EnumValueReceivedFromIntegerExceptionConversion.ConvertEnumValueReceivedFromIntegerException(ex, someFormOfIdentifierForTheProblemOccurrence, httpProblemTypeProvider);
	}
}
