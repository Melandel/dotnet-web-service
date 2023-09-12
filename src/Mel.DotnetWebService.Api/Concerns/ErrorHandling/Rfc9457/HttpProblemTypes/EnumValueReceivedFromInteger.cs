using System.Net;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.HttpProblemTypes;

record EnumValueReceivedFromInteger : HttpProblemType
{
	public EnumValueReceivedFromInteger(Uri uri)
	: base(
		uri,
		HttpStatusCode.BadRequest,
		title: "An enum value was passed as integer instead of being passed as a string.",
		description: "https://stackoverflow.com/questions/49562774/what-is-the-best-way-to-prohibit-integer-value-for-enum-actions-parameter",
		HttpProblemTypeExtensionMember.IntegerValue,
		HttpProblemTypeExtensionMember.EnumParameterName,
		HttpProblemTypeExtensionMember.SupportedEnumValues)
	{
	}
}
