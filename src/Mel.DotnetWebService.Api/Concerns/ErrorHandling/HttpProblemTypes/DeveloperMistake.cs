using System.Net;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.HttpProblemTypes;

public record DeveloperMistake : HttpProblemType
{
	public DeveloperMistake(Uri uri, IWebHostEnvironment hostEnvironment)
	: base(
		uri,
		HttpStatusCode.InternalServerError,
		title: "An unexpected error took place.",
		description: "An error, flaw or fault in the design, development, or operation of computer software caused the application to produce an incorrect or unexpected result, or to behave in unintended ways.",
		hostEnvironment.IsProduction()
			? new[]
			{
				HttpProblemTypeExtensionMember.ExceptionsTypes,
				HttpProblemTypeExtensionMember.ExceptionsAggregatedMessages,
				HttpProblemTypeExtensionMember.ExceptionsAggregatedData
			}
			: new[]
			{
				HttpProblemTypeExtensionMember.StackTrace,
				HttpProblemTypeExtensionMember.ExceptionsTypes,
				HttpProblemTypeExtensionMember.ExceptionsAggregatedMessages,
				HttpProblemTypeExtensionMember.ExceptionsAggregatedData
			})
	{
	}
}
