using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class ShortInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<short>
{
	protected ShortInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<short> exampleValues) : base(exampleValues) { }
	public static readonly ShortInstanciationExpressionBuilder Instance = new(ExampleValues.ForShortType);
}
