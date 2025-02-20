using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class UShortExampleValueGenerator : NativeValueExampleGenerator<ushort>
{
	protected UShortExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<ushort> exampleValues) : base(exampleValues) { }
	public static readonly UShortExampleValueGenerator Instance = new(ExampleValues.ForUShortType);
}
