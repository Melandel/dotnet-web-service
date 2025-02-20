using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyNegativeDouble : ConstrainedDouble, IConstrainedDouble<StrictlyNegativeDouble>
{
	public static ExampleValues<double> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: [ -24d, -60d, -120d, -double.Epsilon ],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document(0d,             "Value must be less than zero"),
			ConstraintViolationExample.Document(double.Epsilon, "Value must be less than zero")
		]);

	StrictlyNegativeDouble(double value) : base(value)
	{
		if (Value >= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyNegativeDouble>(nameof(Value), value, "@member must be less than zero");
		}
	}

	public static StrictlyNegativeDouble ApplyConstraintsTo(double scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyNegativeDouble>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyNegativeDouble>(defect, scalarValue); }
	}
}
