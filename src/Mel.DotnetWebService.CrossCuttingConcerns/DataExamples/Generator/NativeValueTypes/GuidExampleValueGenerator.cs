using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class GuidExampleValueGenerator : NativeValueExampleGenerator<Guid>
{
	protected GuidExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<Guid> exampleValues) : base(exampleValues) { }
	public static readonly GuidExampleValueGenerator Instance = new(ExampleValues.ForGuidType);
}
