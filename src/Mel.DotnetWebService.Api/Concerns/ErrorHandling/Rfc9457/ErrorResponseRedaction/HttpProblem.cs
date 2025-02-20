namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

record HttpProblem
{
	public HttpProblemType Type { get; }
	public HttpProblemOccurrence Occurrence { get; }
	HttpProblem(HttpProblemType type, HttpProblemOccurrence occurrence)
	{
		foreach (var extensionMemberName in type.DefiningSpecification.ExpectedExtensionMembers)
		{
			if (!occurrence.ProblemSpecificInformation.ContainsKey(extensionMemberName))
			{
				throw ObjectConstructionException.WhenConstructingAMemberFor<HttpProblem>(nameof(Type), type, $"the extension member {extensionMemberName} is missing while being required when a {type.GetType().Name}-type problem occurs.");
			}
		}
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
			Detail = Occurrence.FeedbackForApiUser,
			Instance = Occurrence.Id?.ToString()
		};

		foreach (var problemInformation in Occurrence.ProblemSpecificInformation)
		{
			var extensionMemberName = problemInformation.Key.ToString();
			var extensionMemberValue = problemInformation.Value;

			problemDetails.Extensions.Add(extensionMemberName, extensionMemberValue);
		}
		return problemDetails;
	}
};
