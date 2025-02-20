using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class NUIntInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<nuint>
{
	protected NUIntInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<nuint> exampleValues) : base(exampleValues) { }
	public static readonly NUIntInstanciationExpressionBuilder Instance = new(ExampleValues.ForNUIntType);
}
