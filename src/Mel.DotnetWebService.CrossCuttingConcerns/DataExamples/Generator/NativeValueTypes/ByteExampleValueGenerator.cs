using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class ByteExampleValueGenerator : NativeValueExampleGenerator<byte>
{
	protected ByteExampleValueGenerator(ArrayOfUniqueValuesWithAtLeast2Items<byte> exampleValues) : base(exampleValues) { }
	public static readonly ByteExampleValueGenerator Instance = new(ExampleValues.ForByteType);
}
