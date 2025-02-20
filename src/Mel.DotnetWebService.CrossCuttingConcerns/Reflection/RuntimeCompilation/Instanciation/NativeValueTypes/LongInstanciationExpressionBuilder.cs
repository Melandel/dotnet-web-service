using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class LongInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<long>
{
	protected LongInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<long> exampleValues) : base(exampleValues) { }
	public static readonly LongInstanciationExpressionBuilder Instance = new(ExampleValues.ForLongType);
}
