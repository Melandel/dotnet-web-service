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

	public static HttpProblem FromDeveloperMistake(Exception exception, HttpProblemType developerMistakeProblemType, Uri someFormOfIdentifierForTheProblemOccurrence)
	=> From(
		developerMistakeProblemType,
		HttpProblemOccurrence.FromDeveloperMistakeInThisApi(
			id: someFormOfIdentifierForTheProblemOccurrence,
			apologeticStatement: "Oops! Something wrong happened on our side...",
			problemSpecificInformation: developerMistakeProblemType.DefiningSpecification.ExpectedExtensionMembers
				.ToDictionary<HttpProblemTypeExtensionMember, HttpProblemTypeExtensionMember, object>(
					keySelector: extensionMemberName => extensionMemberName,
					elementSelector: extensionMemberName => extensionMemberName switch
					{
						HttpProblemTypeExtensionMember.StackTrace => ExceptionExtensionMethods.GetStackTraceFromAllInnerExceptions(exception),
						HttpProblemTypeExtensionMember.ExceptionsTypes => ExceptionExtensionMethods.GetConcatenatedExceptionTypesFromAllInnerExceptions(exception),
						HttpProblemTypeExtensionMember.ExceptionsAggregatedMessages => ExceptionExtensionMethods.GetConcatenatedMessagesFromAllInnerExceptions(exception),
						HttpProblemTypeExtensionMember.ExceptionsAggregatedData => ExceptionExtensionMethods.GetConcatenatedDataFromAllInnerExceptions(exception),
						var extensionMemberRequiredButMissing => throw new NotImplementedException($"the extension member {extensionMemberRequiredButMissing.ToString()} is not supported when a {developerMistakeProblemType.GetType().Name}-type problem occurs.")
					})));

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
