using System.Globalization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class RegionInfoExampleValueGenerator : SystemTypeThatCanThrowOnInstanciationValueExampleGenerator<RegionInfo>
{
	protected RegionInfoExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<RegionInfo> exampleValues) : base(exampleValues) { }
	public static readonly RegionInfoExampleValueGenerator Instance = new(ExampleValues.ForRegionInfoType);
}
