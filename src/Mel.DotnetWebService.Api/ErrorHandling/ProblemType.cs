using System.Net;

namespace Mel.DotnetWebService.Api.ErrorHandling;

class ProblemType
{
	public Uri Id { get; }
	public string Name { get; }
	public HttpStatusCode HttpStatus { get; }
	ProblemType(Uri id, string name, HttpStatusCode httpStatus)
	{
		Id = id;
		Name = name;
		HttpStatus = httpStatus;
	}

	public static ProblemType DeveloperMistake
	=> new(
		new Uri("https://en.wikipedia.org/wiki/Software_bug"),
		"Developer mistake",
		HttpStatusCode.InternalServerError);
}
