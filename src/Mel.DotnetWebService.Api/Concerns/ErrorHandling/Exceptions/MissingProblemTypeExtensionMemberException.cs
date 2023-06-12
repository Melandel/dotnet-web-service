namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Exceptions;

class MissingProblemTypeExtensionMemberException : Exception
{
	MissingProblemTypeExtensionMemberException(string message, Exception? innerException = null) : base(message, innerException)
	{
	}

	public static MissingProblemTypeExtensionMemberException WhenBuildingHttpProblemOccurrence(
		Type httpProblemType,
		HttpProblemTypeExtensionMember extensionMemberRequiredButMissing,
		Exception? innerException = null)
	=> new(
		$"The extension member {extensionMemberRequiredButMissing.ToString()} is missing while being required when a {httpProblemType.Name}-type problem occurs.",
		innerException);
}
