using System.Collections.ObjectModel;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.ErrorResponseRedaction;

class HttpProblemOccurrence
{
	public Uri? Id { get; }
	public Statement FeedbackForApiUser { get; }
	public IReadOnlyDictionary<HttpProblemTypeExtensionMember, object> ProblemSpecificInformation { get; }
	HttpProblemOccurrence(
		Uri? id,
		Statement feedbackForApiUser,
		ReadOnlyDictionary<HttpProblemTypeExtensionMember, object> problemSpecificInformation)
	{
		Id = id;
		FeedbackForApiUser = feedbackForApiUser;
		ProblemSpecificInformation = problemSpecificInformation;
	}

	public static HttpProblemOccurrence FromDeveloperMistakeInThisApi(
		Uri id,
		string apologeticStatement,
		IDictionary<HttpProblemTypeExtensionMember, object> problemSpecificInformation)
	=> new(
		id,
		Statement.FromString(apologeticStatement),
		problemSpecificInformation.AsReadOnly());
}
