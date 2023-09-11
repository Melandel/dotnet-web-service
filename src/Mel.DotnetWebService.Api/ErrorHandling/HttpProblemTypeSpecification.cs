namespace Mel.DotnetWebService.Api.ErrorHandling;

public record HttpProblemTypeSpecification
{
	public string Description { get; }
	public IEnumerable<DebuggingInformationName> DebuggingInformationToProvide { get; }
	public HttpProblemTypeSpecification(string description, DebuggingInformationName[] debuggingInformationToProvide)
	{
		Description = description;
		DebuggingInformationToProvide = debuggingInformationToProvide;
	}

	public static HttpProblemTypeSpecification From(
		string description,
		IEnumerable<DebuggingInformationName> debuggingInformationToProvide)
	=> new (description, debuggingInformationToProvide.ToArray());
}
