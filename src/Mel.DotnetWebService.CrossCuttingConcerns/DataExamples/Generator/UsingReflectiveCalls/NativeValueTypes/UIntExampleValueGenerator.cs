using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingReflectiveCalls;

class UIntExampleValueGenerator : NativeValueExampleGenerator<uint>
{
	protected UIntExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<uint> exampleValues) : base(exampleValues) { }
	public static readonly UIntExampleValueGenerator Instance = new(ExampleValues.ForUIntType);
}
