using System.Text.Json.Serialization;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Serialization;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.ErrorResponseRedaction;

public record HttpProblemTypeSpecification
{
	public string Description { get; }

	[JsonConverter(typeof(HttpProblemTypeExtensionMemberConverter))]
	public IEnumerable<HttpProblemTypeExtensionMember> ExpectedExtensionMembers { get; }
	public HttpProblemTypeSpecification(string description, HttpProblemTypeExtensionMember[] expectedExtensionMembers)
	{
		Description = description;
		ExpectedExtensionMembers = expectedExtensionMembers;
	}

	public static HttpProblemTypeSpecification From(
		string description,
		IEnumerable<HttpProblemTypeExtensionMember> expectedExtensionMembers)
	=> new (description, expectedExtensionMembers.ToArray());
}
