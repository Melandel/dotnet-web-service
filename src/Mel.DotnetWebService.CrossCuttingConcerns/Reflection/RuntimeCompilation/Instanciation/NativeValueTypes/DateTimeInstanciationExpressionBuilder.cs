using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class DateTimeInstanciationExpressionBuilder : NativeValueInstanciationExpressionBuilder<DateTime>
{
	protected DateTimeInstanciationExpressionBuilder(ArrayOfUniqueValuesWithAtLeast2Items<DateTime> exampleValues) : base(exampleValues) { }
	public static readonly DateTimeInstanciationExpressionBuilder Instance = new(ExampleValues.ForDateTimeType);
}
