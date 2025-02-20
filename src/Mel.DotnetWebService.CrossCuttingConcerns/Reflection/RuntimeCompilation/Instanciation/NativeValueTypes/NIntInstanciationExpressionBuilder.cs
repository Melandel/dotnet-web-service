using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class NIntInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<nint>
{
	protected NIntInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<nint> exampleValues) : base(exampleValues) { }
	public static readonly NIntInstanciationExpressionBuilder Instance = new(ExampleValues.ForNIntType);
}
