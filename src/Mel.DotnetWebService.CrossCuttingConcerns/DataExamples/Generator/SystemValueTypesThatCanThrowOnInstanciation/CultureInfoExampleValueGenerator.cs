using System.Globalization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class CultureInfoExampleValueGenerator : SystemTypeThatCanThrowOnInstanciationValueExampleGenerator<CultureInfo>
{
	protected CultureInfoExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<CultureInfo> exampleValues) : base(exampleValues) { }
	public static readonly CultureInfoExampleValueGenerator Instance = new(ExampleValues.ForCultureInfoType);
}
