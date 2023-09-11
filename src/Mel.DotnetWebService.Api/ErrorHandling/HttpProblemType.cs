using System.Net;

namespace Mel.DotnetWebService.Api.ErrorHandling;

public record HttpProblemType
{
	public Uri Uri { get; init; }
	public string Title { get; init; }
	public HttpStatusCode? RecommendedHttpStatusCode { get; init; }
	public HttpProblemTypeSpecification DefiningSpecification { get; init; }

	[Newtonsoft.Json.JsonConstructor]
	HttpProblemType(
			Uri uri,
			string title,
			HttpStatusCode? recommendedHttpStatusCode,
			HttpProblemTypeSpecification definingSpecification)
	{
		Uri = uri;
		Title = title;
		RecommendedHttpStatusCode = recommendedHttpStatusCode;
		DefiningSpecification = definingSpecification;
	}

	public static HttpProblemType Create(
		Uri uri,
		HttpStatusCode? recommendedHttpStatusCode,
		string title,
		string description,
		params DebuggingInformationName[] debuggingInformationToProvide)
	=> new(
		uri,
		title,
		recommendedHttpStatusCode,
		HttpProblemTypeSpecification.From(description, debuggingInformationToProvide));
}
