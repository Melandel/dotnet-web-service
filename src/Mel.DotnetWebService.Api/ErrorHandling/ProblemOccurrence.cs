namespace Mel.DotnetWebService.Api.ErrorHandling;

class ProblemOccurrence
{
	public Uri? Id { get; }
	public string ExplanationThatShouldHelpTheClientCorrectTheProblem { get; }
	ProblemOccurrence(Uri? id, string explanationThatShouldHelpTheClientCorrectTheProblem)
	{
		Id = id;
		ExplanationThatShouldHelpTheClientCorrectTheProblem = explanationThatShouldHelpTheClientCorrectTheProblem;
	}

	public static ProblemOccurrence FromIdAndExplanationThatShouldHelpTheClientCorrectTheProblem(
		Uri id,
		string explanationThatShouldHelpTheClientCorrectTheProblem)
	=> new(id, explanationThatShouldHelpTheClientCorrectTheProblem);

	public static ProblemOccurrence FromExplanationThatShouldHelpTheClientCorrectTheProblem(
		string explanationThatShouldHelpTheClientCorrectTheProblem)
	=> new(null, explanationThatShouldHelpTheClientCorrectTheProblem);
}
