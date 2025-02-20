using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class ByteInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<byte>
{
	protected ByteInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<byte> exampleValues) : base(exampleValues) { }
	public static readonly ByteInstanciationExpressionBuilder Instance = new(ExampleValues.ForByteType);
}
