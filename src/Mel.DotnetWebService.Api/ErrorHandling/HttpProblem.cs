namespace Mel.DotnetWebService.Api.ErrorHandling;

record HttpProblem
{
	public HttpProblemType Type { get; }
	public HttpProblemOccurrence Occurrence { get; }
	HttpProblem(HttpProblemType type, HttpProblemOccurrence occurrence)
	{
		Type = type;
		Occurrence = occurrence;
	}

	public static HttpProblem From(HttpProblemType httpProblemType, HttpProblemOccurrence httpProblemOccurrence)
	=> new(httpProblemType, httpProblemOccurrence);

	public Microsoft.AspNetCore.Mvc.ProblemDetails ToProblemDetails()
	{
		var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
		{
			Type = Type.Uri.ToString(),
			Title = Type.Title,
			Status = (int?) Type.RecommendedHttpStatusCode,
			Detail = Occurrence.ExplanationThatShouldHelpTheClientUnderstandTheApi,
			Instance = Occurrence.Id?.ToString()
		};

		foreach (var debugInformationToProvide in Occurrence.DebuggingInformationToProvide)
		{
			var debugInfoName = debugInformationToProvide.Key.ToString();
			var debugInfoValue = debugInformationToProvide.Value;

			problemDetails.Extensions.Add(debugInfoName, debugInfoValue);
		}
		return problemDetails;
	}
};
