using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingReflectiveCalls;

class FloatExampleValueGenerator : NativeValueExampleGenerator<float>
{
	protected FloatExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<float> exampleValues) : base(exampleValues) { }
	public static readonly FloatExampleValueGenerator Instance = new(ExampleValues.ForFloatType);
}
