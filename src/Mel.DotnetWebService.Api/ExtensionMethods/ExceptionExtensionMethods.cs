using Mel.DotnetWebService.Api.ErrorHandling;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ExceptionExtensionMethods
{
	internal static Problem ToProblem(this Exception exception)
	=> exception switch
	{
		_ => DeveloperMistake.FromStackTrace(exception.StackTrace)
	};
}
