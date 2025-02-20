using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class NUIntExampleValueGenerator : NativeValueExampleGenerator<nuint>
{
	protected NUIntExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<nuint> exampleValues) : base(exampleValues) { }
	public static readonly NUIntExampleValueGenerator Instance = new(ExampleValues.ForNUIntType);
}
