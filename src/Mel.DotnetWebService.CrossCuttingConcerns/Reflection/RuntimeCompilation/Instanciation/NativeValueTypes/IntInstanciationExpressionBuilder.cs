using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class IntInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<int>
{
	protected IntInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<int> exampleValues) : base(exampleValues) { }
	public static readonly IntInstanciationExpressionBuilder Instance = new(ExampleValues.ForIntType);
}
