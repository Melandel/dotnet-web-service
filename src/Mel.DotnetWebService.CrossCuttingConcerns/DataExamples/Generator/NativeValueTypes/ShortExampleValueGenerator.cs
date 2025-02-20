using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class ShortExampleValueGenerator : NativeValueExampleGenerator<short>
{
	protected ShortExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<short> exampleValues) : base(exampleValues) { }
	public static readonly ShortExampleValueGenerator Instance = new(ExampleValues.ForShortType);
}
