using System.Collections.ObjectModel;

namespace Mel.DotnetWebService.Api.ErrorHandling;

class HttpProblemOccurrence
{
	public Uri? Id { get; }
	public string ExplanationThatShouldHelpTheClientUnderstandTheApi { get; }
	public IReadOnlyDictionary<DebuggingInformationName, object> DebuggingInformationToProvide { get; }
	HttpProblemOccurrence(
		Uri? id,
		string explanationThatShouldHelpTheClientUnderstandTheApi,
		ReadOnlyDictionary<DebuggingInformationName, object> debuggingInformationToProvide)
	{
		Id = id;
		ExplanationThatShouldHelpTheClientUnderstandTheApi = explanationThatShouldHelpTheClientUnderstandTheApi;
		DebuggingInformationToProvide = debuggingInformationToProvide;
	}

	public static HttpProblemOccurrence FromIdAndExplanationThatShouldHelpTheClientUnderstandTheApi(
		Uri id,
		string explanationThatShouldHelpTheClientUnderstandTheApi,
		IDictionary<DebuggingInformationName, object> debuggingInformationToProvide)
	=> new(id, explanationThatShouldHelpTheClientUnderstandTheApi, debuggingInformationToProvide.AsReadOnly());
}
