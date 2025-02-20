using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class UIntInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<uint>
{
	protected UIntInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<uint> exampleValues) : base(exampleValues) { }
	public static readonly UIntInstanciationExpressionBuilder Instance = new(ExampleValues.ForUIntType);
}
