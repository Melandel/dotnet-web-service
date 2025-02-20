using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class DoubleExampleValueGenerator : NativeValueExampleGenerator<double>
{
	protected DoubleExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<double> exampleValues) : base(exampleValues) { }
	public static readonly DoubleExampleValueGenerator Instance = new(ExampleValues.ForDoubleType);
}
