using Mel.DotnetWebService.Api.Concerns.ErrorHandling.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ExceptionExtensionMethods
{
	public static bool TryConversionToHttpProblem(this Exception exception, HttpContext httpContext, out HttpProblem httpProblem)
	=> Concerns.ErrorHandling.Integration.ExceptionAnalysis.TryConvertingExceptionToHttpProblem(exception, httpContext, out httpProblem);

	public static string GetStackTraceFromAllInnerExceptions(this Exception exception)
	=> Concerns.ErrorHandling.Integration.ExceptionAnalysis.GetStackTraceFromAllInnerExceptions(exception);

	public static string GetConcatenatedExceptionTypesFromAllInnerExceptions(this Exception exception)
	=> Concerns.ErrorHandling.Integration.ExceptionAnalysis.GetConcatenatedExceptionTypesFromAllInnerExceptions(exception);

	public static string GetConcatenatedMessagesFromAllInnerExceptions(this Exception exception)
	=> Concerns.ErrorHandling.Integration.ExceptionAnalysis.GetConcatenatedMessagesFromAllInnerExceptions(exception);

	public static Dictionary<object, object?> GetConcatenatedDataFromAllInnerExceptions(this Exception exception)
	=> Concerns.ErrorHandling.Integration.ExceptionAnalysis.GetConcatenatedDataFromAllInnerExceptions(exception);
}
