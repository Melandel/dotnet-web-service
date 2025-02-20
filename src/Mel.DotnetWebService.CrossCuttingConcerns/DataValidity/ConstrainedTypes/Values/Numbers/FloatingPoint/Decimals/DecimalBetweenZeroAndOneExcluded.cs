using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class DecimalBetweenZeroAndOneExcluded : ConstrainedDecimal, IConstrainedDecimal<DecimalBetweenZeroAndOneExcluded>
{
	public static ExampleValues<decimal> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: [ 0.5m, 0.24m, 0.6m, 0.01m, 0.99m ],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document(0m, "Value must be between 0 (excluded) and 1 (excluded)"),
			ConstraintViolationExample.Document(1m, "Value must be between 0 (excluded) and 1 (excluded)"),
		]);

	DecimalBetweenZeroAndOneExcluded(decimal value) : base(value)
	{
		if (Value is >=1 or <=0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<DecimalBetweenZeroAndOneExcluded>(nameof(Value), value, "@member must be between 0 (excluded) and 1 (excluded)");
		}
	}

	public static DecimalBetweenZeroAndOneExcluded ApplyConstraintsTo(decimal scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<DecimalBetweenZeroAndOneExcluded>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<DecimalBetweenZeroAndOneExcluded>(defect, scalarValue); }
	}
}
