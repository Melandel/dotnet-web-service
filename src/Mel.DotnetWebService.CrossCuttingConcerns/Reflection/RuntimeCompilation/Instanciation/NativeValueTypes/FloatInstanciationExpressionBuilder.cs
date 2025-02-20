using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class FloatInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<float>
{
	protected FloatInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<float> exampleValues) : base(exampleValues) { }
	public static readonly FloatInstanciationExpressionBuilder Instance = new(ExampleValues.ForFloatType);
}
