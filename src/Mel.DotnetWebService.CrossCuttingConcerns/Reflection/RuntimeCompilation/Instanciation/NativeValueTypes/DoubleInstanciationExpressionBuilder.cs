using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class DoubleInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<double>
{
	protected DoubleInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<double> exampleValues) : base(exampleValues) { }
	public static readonly DoubleInstanciationExpressionBuilder Instance = new(ExampleValues.ForDoubleType);
}
