namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling;

public record HttpProblemTypeSpecification
{
	public string Description { get; }
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
