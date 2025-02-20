namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroSByte : ConstrainedSByte, IConstrainedSByte<NonZeroSByte>
{
	public static ExampleValues<sbyte> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new sbyte[] { 1, 2, -1, 127, -128 },
		errorMessagesByInvalidValue: new Dictionary<sbyte, string>
		{
			{ 0, "Value must not be zero" }
		});

	NonZeroSByte(sbyte value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroSByte>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroSByte ApplyConstraintsTo(sbyte scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroSByte>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroSByte>(defect, scalarValue); }
	}
}
