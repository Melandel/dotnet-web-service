using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class DecimalInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<decimal>
{
	protected DecimalInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<decimal> exampleValues) : base(exampleValues) { }
	public static readonly DecimalInstanciationExpressionBuilder Instance = new(ExampleValues.ForDecimalType);
}
