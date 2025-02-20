using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class BoolInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<bool>
{
	protected BoolInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<bool> exampleValues) : base(exampleValues) { }
	public static readonly BoolInstanciationExpressionBuilder Instance = new(ExampleValues.ForBoolType);
}
