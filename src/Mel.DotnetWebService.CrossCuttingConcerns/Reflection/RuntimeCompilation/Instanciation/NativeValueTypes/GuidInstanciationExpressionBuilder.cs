using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class GuidInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<Guid>
{
	protected GuidInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<Guid> exampleValues) : base(exampleValues) { }
	public static readonly GuidInstanciationExpressionBuilder Instance = new(ExampleValues.ForGuidType);
}
