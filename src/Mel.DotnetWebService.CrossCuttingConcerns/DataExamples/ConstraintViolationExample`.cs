namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

public class ConstraintViolationExample<TValue> : ConstraintViolationExample
{
	public override object? ExampleValue => Value;
	readonly TValue? Value;
	ConstraintViolationExample(TValue value, string errorMessage)
		: base(errorMessage)
	{
		Value = value;
	}
	public static ConstraintViolationExample<TValue> Document(TValue exampleValue, string errorMessage)
	=> new(exampleValue, errorMessage);
}
