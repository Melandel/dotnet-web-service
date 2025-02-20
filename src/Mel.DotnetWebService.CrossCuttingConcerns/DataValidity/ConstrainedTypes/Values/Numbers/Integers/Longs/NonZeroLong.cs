namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroLong : ConstrainedLong, IConstrainedLong<NonZeroLong>
{
	public static ExampleValues<long> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new long[] { 1, 2, -1, 9223372036854775807, -9223372036854775808 },
		errorMessagesByInvalidValue: new Dictionary<long, string>
		{
			{ 0, "Value must not be zero" }
		});

	NonZeroLong(long value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroLong>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroLong ApplyConstraintsTo(long scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroLong>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroLong>(defect, scalarValue); }
	}
}
