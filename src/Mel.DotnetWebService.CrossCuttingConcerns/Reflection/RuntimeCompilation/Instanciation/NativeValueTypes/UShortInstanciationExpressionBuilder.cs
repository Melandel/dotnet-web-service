using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class UShortInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<ushort>
{
	protected UShortInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<ushort> exampleValues) : base(exampleValues) { }
	public static readonly UShortInstanciationExpressionBuilder Instance = new(ExampleValues.ForUShortType);
}
