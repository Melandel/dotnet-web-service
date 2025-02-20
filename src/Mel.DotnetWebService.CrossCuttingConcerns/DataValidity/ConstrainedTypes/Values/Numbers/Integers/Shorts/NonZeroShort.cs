namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroShort : ConstrainedShort, IConstrainedShort<NonZeroShort>
{
	public static ExampleValues<short> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new short[] { 1, 2, -1, 32767, -32768 },
		errorMessagesByInvalidValue: new Dictionary<short, string>
		{
			{ 0, "Value must not be zero" }
		});

	NonZeroShort(short value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroShort>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroShort ApplyConstraintsTo(short scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroShort>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroShort>(defect, scalarValue); }
	}
}
