using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class NIntExampleValueGenerator : NativeValueExampleGenerator<nint>
{
	protected NIntExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<nint> exampleValues) : base(exampleValues) { }
	public static readonly NIntExampleValueGenerator Instance = new(ExampleValues.ForNIntType);
}
