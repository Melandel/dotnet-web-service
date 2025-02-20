using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class LongExampleValueGenerator : NativeValueExampleGenerator<long>
{
	protected LongExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<long> exampleValues) : base(exampleValues) { }
	public static readonly LongExampleValueGenerator Instance = new(ExampleValues.ForLongType);
}
