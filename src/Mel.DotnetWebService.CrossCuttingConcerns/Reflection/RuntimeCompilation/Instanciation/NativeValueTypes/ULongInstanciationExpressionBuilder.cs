using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class ULongInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<ulong>
{
	protected ULongInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<ulong> exampleValues) : base(exampleValues) { }
	public static readonly ULongInstanciationExpressionBuilder Instance = new(ExampleValues.ForULongType);
}
