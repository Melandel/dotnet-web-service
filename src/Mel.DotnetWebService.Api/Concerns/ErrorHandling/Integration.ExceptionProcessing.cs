using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling;

static partial class Integration
{
	public static class ExceptionProcessing
	{
		public static bool TryConvertingExceptionToHttpProblem(Exception exception, HttpContext httpContext, out HttpProblem httpProblem)
		=> Rfc9457.ExceptionProcessing.ExceptionToHttpProblemConversion.TryConverting(exception, httpContext, out httpProblem);
	}
}
