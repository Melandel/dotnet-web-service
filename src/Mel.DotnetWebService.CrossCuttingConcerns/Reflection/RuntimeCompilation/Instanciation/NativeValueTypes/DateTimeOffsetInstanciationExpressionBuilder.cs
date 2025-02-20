using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class DateTimeOffsetInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<DateTimeOffset>
{
	protected DateTimeOffsetInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<DateTimeOffset> exampleValues) : base(exampleValues) { }
	public static readonly DateTimeOffsetInstanciationExpressionBuilder Instance = new(ExampleValues.ForDateTimeOffsetType);
}
