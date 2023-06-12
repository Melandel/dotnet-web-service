using System.Net;

namespace Mel.DotnetWebService.Api.ErrorHandling;

public record HttpProblemType
{
	public Uri Uri { get; init; }
	public string Title { get; init; }
	public HttpStatusCode? RecommendedHttpStatusCode { get; init; }
	public string DefiningSpecification { get; init; }

	HttpProblemType(
		Uri uri,
		string title,
		HttpStatusCode? recommendedHttpStatusCode,
		string definingSpecification)
	{
		Uri = uri;
		Title = title;
		RecommendedHttpStatusCode = recommendedHttpStatusCode;
		DefiningSpecification = definingSpecification;
	}

	public static HttpProblemType CreateWithDefiningSpecification(
		Uri uri,
		HttpStatusCode? recommendedHttpStatusCode,
		string title,
		string definingSpecification)
	=> new HttpProblemType(uri, title, recommendedHttpStatusCode, definingSpecification);

	public static HttpProblemType CreateWithoutDefiningSpecification(
		Uri uri,
		HttpStatusCode? recommendedHttpStatusCode,
		string title)
	=> new HttpProblemType(uri, title, recommendedHttpStatusCode, title);
}
