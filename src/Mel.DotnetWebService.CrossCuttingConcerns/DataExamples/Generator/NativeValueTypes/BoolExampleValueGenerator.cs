using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class BoolExampleValueGenerator : NativeValueExampleGenerator<bool>
{
	protected BoolExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<bool> exampleValues) : base(exampleValues) { }
	public static readonly BoolExampleValueGenerator Instance = new(ExampleValues.ForBoolType);
}
