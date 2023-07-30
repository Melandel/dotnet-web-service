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

	public static ProblemType EnumValueReceivedFromInteger
	=> new(
		new Uri("https://stackoverflow.com/questions/49562774/what-is-the-best-way-to-prohibit-integer-value-for-enum-actions-parameter"),
		"Enum value received from integer",
		HttpStatusCode.BadRequest);
}
