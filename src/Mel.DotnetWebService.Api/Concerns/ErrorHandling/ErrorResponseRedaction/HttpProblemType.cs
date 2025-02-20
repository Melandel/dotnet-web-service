using System.Net;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Serialization;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.ErrorResponseRedaction;

public abstract record HttpProblemType
{
	public Uri Uri { get; }

	[JsonConverter(typeof(HttpStatusCodeConverter))]
	public HttpStatusCode? RecommendedHttpStatusCode { get; }
	public string Title { get; }
	public HttpProblemTypeSpecification DefiningSpecification { get; }

	[Newtonsoft.Json.JsonConstructor]
	HttpProblemType(
		Uri uri,
		HttpStatusCode? recommendedHttpStatusCode,
		string title,
		HttpProblemTypeSpecification definingSpecification)
	{
		Uri = uri;
		RecommendedHttpStatusCode = recommendedHttpStatusCode;
		Title = title;
		DefiningSpecification = definingSpecification;
	}

	protected HttpProblemType(
		Uri uri,
		HttpStatusCode? recommendedHttpStatusCode,
		string title,
		string description,
		params HttpProblemTypeExtensionMember[] expectedExtensionMembers)
	: this(
		uri,
		recommendedHttpStatusCode,
		title,
		HttpProblemTypeSpecification.From(description, expectedExtensionMembers))
	{
	}
}
