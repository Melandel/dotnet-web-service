using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class ULongExampleValueGenerator : NativeValueExampleGenerator<ulong>
{
	protected ULongExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<ulong> exampleValues) : base(exampleValues) { }
	public static readonly ULongExampleValueGenerator Instance = new(ExampleValues.ForULongType);
}
