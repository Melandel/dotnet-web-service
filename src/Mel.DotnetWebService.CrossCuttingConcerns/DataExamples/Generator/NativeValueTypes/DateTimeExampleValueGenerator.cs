using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class DateTimeExampleValueGenerator : NativeValueExampleGenerator<DateTime>
{
	protected DateTimeExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<DateTime> exampleValues) : base(exampleValues) { }
	public static readonly DateTimeExampleValueGenerator Instance = new(ExampleValues.ForDateTimeType);
}
