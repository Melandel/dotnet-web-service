using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class VersionExampleValueGenerator : SystemTypeThatCanThrowOnInstanciationValueExampleGenerator<Version>
{
	protected VersionExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<Version> exampleValues) : base(exampleValues) { }
	public static readonly VersionExampleValueGenerator Instance = new(ExampleValues.ForVersionType);
}
