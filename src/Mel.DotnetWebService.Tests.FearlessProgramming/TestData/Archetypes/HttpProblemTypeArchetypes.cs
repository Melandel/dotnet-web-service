using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static class HttpProblemTypeArchetype
{
	public record Deserializable : HttpProblemType
	{
		public Deserializable(
			Uri uri,
			HttpStatusCode? recommendedHttpStatusCode,
			string title,
			string description,
			params HttpProblemTypeExtensionMember[] expectedExtensionMembers)
		: base(
			uri,
			recommendedHttpStatusCode,
			title,
			description,
			expectedExtensionMembers ?? Array.Empty<HttpProblemTypeExtensionMember>())
		{
		}
	}
}
