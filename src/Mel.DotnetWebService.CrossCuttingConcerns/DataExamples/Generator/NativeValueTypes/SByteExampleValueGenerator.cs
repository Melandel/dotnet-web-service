using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class SByteExampleValueGenerator : NativeValueExampleGenerator<sbyte>
{
	protected SByteExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<sbyte> exampleValues) : base(exampleValues) { }
	public static readonly SByteExampleValueGenerator Instance = new(ExampleValues.ForSByteType);
}
