namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.Exceptions;

class TestDataIntegrityException : Exception
{
	TestDataIntegrityException(string message, Exception innerException = null) : base(message, innerException)
	{
	}

	public static TestDataIntegrityException GeneratedBy(Type testDataGenerationType, string testDataGenerationMember, string details, Exception innerException = null)
	=> new($"Test data generation {testDataGenerationType.Name}.{testDataGenerationMember} failed: {details}", innerException);
}
