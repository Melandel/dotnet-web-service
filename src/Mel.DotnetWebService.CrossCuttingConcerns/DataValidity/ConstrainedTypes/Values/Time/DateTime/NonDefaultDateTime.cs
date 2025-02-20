namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonDefaultDateTime : ConstrainedDateTime, IConstrainedDateTime<NonDefaultDateTime>
{
	public static ExampleValues<DateTime> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new[]
		{
			new DateTime(year: 2000, month: 2, day: 1, hour: 21, minute: 20, second: 19),
			new DateTime(      2026,        3,      2,       18,         17,         16),
			new DateTime(      2100,        4,      3,       15,         14,         13)
		},
		errorMessagesByInvalidValue: new Dictionary<DateTime, string>
		{
			{ DateTime.MinValue, "Value must not be the first day of year 0001" }
		});

	NonDefaultDateTime(DateTime value) : base(value)
	{
		if (Value is { Year: 1, Month: 1, Day: 1 })
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonDefaultDateTime>(nameof(Value), value, "@member must not be the first day of year 0001");
		}
	}

	public static NonDefaultDateTime ApplyConstraintsTo(DateTime scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonDefaultDateTime>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonDefaultDateTime>(defect, scalarValue); }
	}
}
