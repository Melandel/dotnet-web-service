using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class StringInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<string>
{
	protected StringInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<string> exampleValues) : base(exampleValues) { }
	public static readonly StringInstanciationExpressionBuilder Instance = new(ExampleValues.ForStringType);
}
