using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class DecimalBetweenZeroAndOneIncluded : ConstrainedDecimal, IConstrainedDecimal<DecimalBetweenZeroAndOneIncluded>
{
	public static ExampleValues<decimal> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: [ 0m, 1m, 0.5m, 0.24m, 0.6m ],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document(-0.1m, "Value must be between 0 (included) and 1 (included)"),
			ConstraintViolationExample.Document(1.1m, "Value must be between 0 (included) and 1 (included)"),
		]);

	DecimalBetweenZeroAndOneIncluded(decimal value) : base(value)
	{
		if (Value is >1 or <0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<DecimalBetweenZeroAndOneIncluded>(nameof(Value), value, "@member must be between 0 (included) and 1 (included)");
		}
	}

	public static DecimalBetweenZeroAndOneIncluded ApplyConstraintsTo(decimal scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<DecimalBetweenZeroAndOneIncluded>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<DecimalBetweenZeroAndOneIncluded>(defect, scalarValue); }
	}
}
