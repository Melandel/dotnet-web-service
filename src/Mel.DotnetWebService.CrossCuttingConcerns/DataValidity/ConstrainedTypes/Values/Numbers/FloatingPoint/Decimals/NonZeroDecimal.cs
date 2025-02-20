namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroDecimal : ConstrainedDecimal, IConstrainedDecimal<NonZeroDecimal>
{
	public static ExampleValues<decimal> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new[] { 1m, 2.5m, -1.5m, 79228162514264337593543950335m, -79228162514264337593543950335m },
		errorMessagesByInvalidValue: new Dictionary<decimal, string>
		{
			{ 0, "Value must not be zero" }
		});

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
