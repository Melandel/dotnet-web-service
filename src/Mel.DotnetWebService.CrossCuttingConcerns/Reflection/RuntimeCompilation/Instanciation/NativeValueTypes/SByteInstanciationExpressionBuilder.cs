using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class SByteInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<sbyte>
{
	protected SByteInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<sbyte> exampleValues) : base(exampleValues) { }
	public static readonly SByteInstanciationExpressionBuilder Instance = new(ExampleValues.ForSByteType);
}
