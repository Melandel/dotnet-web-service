using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class PositiveDouble : ConstrainedDouble, IConstrainedDouble<PositiveDouble>
{
	public static ExampleValues<double> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForDoubleType.Where(d => d >= 0d),
		constraintViolationExamples: [ ConstraintViolationExample.Document(-double.Epsilon, "Value must be equal to or greater than zero") ]);

	PositiveDouble(double value) : base(value)
	{
		if (Value < 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<PositiveDouble>(nameof(Value), value, "@member must be equal to or greater than zero");
		}
	}

	public static PositiveDouble ApplyConstraintsTo(double scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<PositiveDouble>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<PositiveDouble>(defect, scalarValue); }
	}
}
