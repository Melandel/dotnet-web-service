namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

public abstract class ConstraintViolationExample
{
	public abstract object? ExampleValue { get; }
	public string ErrorMessage { get; }
	protected ConstraintViolationExample(string errorMessage)
	{
		ErrorMessage = errorMessage;
	}

	public static ConstraintViolationExample<T> Document<T>(T exampleValue, string errorMessage)
	=> ConstraintViolationExample<T>.Document(exampleValue, errorMessage);
}
