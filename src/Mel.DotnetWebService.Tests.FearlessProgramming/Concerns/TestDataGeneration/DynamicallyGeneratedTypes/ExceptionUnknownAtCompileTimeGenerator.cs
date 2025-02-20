namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.DynamicallyGeneratedTypes;

static class ExceptionUnknownAtCompileTimeGenerator
{
	public static Exception GenerateWithMessage(string message)
	{
		var instanciator = ExceptionUnknownAtCompileTime.Instantiator;
		var exception = (Exception) instanciator.Invoke(message);
		return exception;
	}
}
