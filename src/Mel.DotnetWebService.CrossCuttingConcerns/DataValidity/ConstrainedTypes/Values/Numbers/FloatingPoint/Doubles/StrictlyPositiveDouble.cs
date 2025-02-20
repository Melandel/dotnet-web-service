using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyPositiveDouble : ConstrainedDouble, IConstrainedDouble<StrictlyPositiveDouble>
{
	public static ExampleValues<double> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForDoubleType.Where(d => d > 0d),
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document(0d,              "Value must be greater than zero"),
			ConstraintViolationExample.Document(-double.Epsilon, "Value must be greater than zero"),
		]);

	StrictlyPositiveDouble(double value) : base(value)
	{
		if (Value <= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyPositiveDouble>(nameof(Value), value, "@member must be greater than zero");
		}
	}

	public static StrictlyPositiveDouble ApplyConstraintsTo(double scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyPositiveDouble>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyPositiveDouble>(defect, scalarValue); }
	}
}
