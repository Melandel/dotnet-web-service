using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class NativeValueExampleGenerator<T> : NativeValueExampleGenerator
{
	readonly ArrayOfUniqueValuesWithAtLeast2Items<T> ExampleValues;
	protected NativeValueExampleGenerator(ArrayOfUniqueValuesWithAtLeast2Items<T> exampleValues)
	{
		ExampleValues = exampleValues;
	}

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	=> ExampleValues[salt % ExampleValues.Length]!;
}
