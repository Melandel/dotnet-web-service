namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

public static class ExampleValues
{
	public static ExampleValues<T> ValidAndInvalid<T>(IEnumerable<T> validValues, IEnumerable<ConstraintViolationExample<T>> constraintViolationExamples)
	=> ExampleValues<T>.ValidAndInvalid(validValues, constraintViolationExamples);
}
