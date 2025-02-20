using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingReflectiveCalls;

class StringExampleValueGenerator : NativeValueExampleGenerator<string>
{
	protected StringExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<string> exampleValues) : base(exampleValues) { }
	public static readonly StringExampleValueGenerator Instance = new(ExampleValues.ForStringType);
}
