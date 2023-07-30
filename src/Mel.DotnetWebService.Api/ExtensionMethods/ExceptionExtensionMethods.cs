using Mel.DotnetWebService.Api.ErrorHandling;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ExceptionExtensionMethods
{
	internal static Problem ToProblem(this Exception exception)
	=> exception switch
	{
		EnumValueReceivedFromIntegerException ex => EnumValueReceivedFromIntegerProblem.From(ex.IntegerValue, ex.ParameterName, ex.AcceptedEnumValues),
		_ => DeveloperMistake.FromStackTrace(exception.StackTrace)
	};
}
