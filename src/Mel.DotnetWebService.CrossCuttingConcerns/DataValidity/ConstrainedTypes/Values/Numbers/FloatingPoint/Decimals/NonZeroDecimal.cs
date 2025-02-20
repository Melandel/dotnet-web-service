using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroDecimal : ConstrainedDecimal, IConstrainedDecimal<NonZeroDecimal>
{
	public static ExampleValues<decimal> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForDecimalType.Where(d => d != default),
		constraintViolationExamples: [ ConstraintViolationExample.Document(0m, "Value must not be zero") ]);

	NonZeroDecimal(decimal value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroDecimal>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroDecimal ApplyConstraintsTo(decimal scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroDecimal>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroDecimal>(defect, scalarValue); }
	}
}
