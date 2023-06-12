namespace Mel.DotnetWebService.Api.ErrorHandling;

class DeveloperMistake : Problem
{
	public string? StackTrace { get; }
	public const string StackTraceDebuggingInformationKey = "stacktrace";
	protected override IDictionary<string, object?> DebuggingInformation
	=> new Dictionary<string, object?>
	{
		{ StackTraceDebuggingInformationKey, StackTrace }
	};

	DeveloperMistake(ProblemType type, ProblemOccurrence occurrence, string? stackTrace)
	: base(type, occurrence)
	{
		StackTrace = stackTrace;
	}

	public static DeveloperMistake FromStackTrace(string? stackTrace)
	=> new(
		ProblemType.DeveloperMistake,
		ProblemOccurrence.FromExplanationThatShouldHelpTheClientCorrectTheProblem("An unexpected error took place."),
		stackTrace);
}
