using System.Net;

namespace Mel.DotnetWebService.Api.ErrorHandling;

abstract class Problem
{
	readonly ProblemType _type;
	public Uri Type => _type.Id;
	public string Title => _type.Name;
	public HttpStatusCode Status => _type.HttpStatus;

	readonly ProblemOccurrence _occurrence;
	public Uri? Instance => _occurrence.Id;
	public string Detail => _occurrence.ExplanationThatShouldHelpTheClientCorrectTheProblem;

	protected abstract IDictionary<string, object?> DebuggingInformation { get; }

	protected Problem(ProblemType type, ProblemOccurrence occurrence)
	{
		_type = type;
		_occurrence = occurrence;
	}

	public Microsoft.AspNetCore.Mvc.ProblemDetails ToProblemDetails()
	{
		var problemDetails = ConvertToProblemDetails();
		foreach (var debugInformation in DebuggingInformation)
		{
			problemDetails.Extensions.Add(debugInformation);
		}
		return problemDetails;
	}

	Microsoft.AspNetCore.Mvc.ProblemDetails ConvertToProblemDetails()
	=> new Microsoft.AspNetCore.Mvc.ProblemDetails
	{
		Type = Type.ToString(),
		Title = Title,
		Status = (int) Status,
		Detail = Detail,
		Instance = Instance?.ToString()
	};
};
