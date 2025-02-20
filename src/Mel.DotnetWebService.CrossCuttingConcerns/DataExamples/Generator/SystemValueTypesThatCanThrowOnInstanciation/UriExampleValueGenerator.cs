using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class UriExampleValueGenerator : SystemTypeThatCanThrowOnInstanciationValueExampleGenerator<Uri>
{
	protected UriExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<Uri> exampleValues) : base(exampleValues) { }
	public static readonly UriExampleValueGenerator Instance = new(ExampleValues.ForUriType);
}
