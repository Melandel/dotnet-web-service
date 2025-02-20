using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class DecimalExampleValueGenerator : NativeValueExampleGenerator<decimal>
{
	protected DecimalExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<decimal> exampleValues) : base(exampleValues) { }
	public static readonly DecimalExampleValueGenerator Instance = new(ExampleValues.ForDecimalType);
}
