using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class IntExampleValueGenerator : NativeValueExampleGenerator<int>
{
	protected IntExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<int> exampleValues) : base(exampleValues) { }
	public static readonly IntExampleValueGenerator Instance = new(ExampleValues.ForIntType);
}
