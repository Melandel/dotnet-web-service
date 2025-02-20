namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroUInt : ConstrainedUInt, IConstrainedUInt<NonZeroUInt>
{
	public static ExampleValues<uint> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new uint[] { 1, 2, 3, 4294967295 },
		errorMessagesByInvalidValue: new Dictionary<uint, string>
		{
			{ 0, "Value must not be zero" }
		});

	NonZeroUInt(uint value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroUInt>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroUInt ApplyConstraintsTo(uint scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroUInt>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroUInt>(defect, scalarValue); }
	}
}
