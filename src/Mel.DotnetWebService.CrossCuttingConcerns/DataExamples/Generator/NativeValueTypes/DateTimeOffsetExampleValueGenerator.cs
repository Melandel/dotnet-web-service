using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class DateTimeOffsetExampleValueGenerator : NativeValueExampleGenerator<DateTimeOffset>
{
	protected DateTimeOffsetExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<DateTimeOffset> exampleValues) : base(exampleValues) { }
	public static readonly DateTimeOffsetExampleValueGenerator Instance = new(ExampleValues.ForDateTimeOffsetType);
}
