using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonDefaultDateTimeOffset : ConstrainedDateTimeOffset, IConstrainedDateTimeOffset<NonDefaultDateTimeOffset>
{
	public static ExampleValues<DateTimeOffset> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new[]
		{
			new DateTimeOffset(year: 2010, month: 2, day: 1, hour: 6, minute: 5, second: 4, offset: TimeSpan.FromHours(1)),
			new DateTimeOffset(      2025,        3,      2,       9,         8,         7,         TimeSpan.FromHours(-1)),
			new DateTimeOffset(      2175,        4,      3,      12,        11,        10,         TimeSpan.FromHours(4))
		},
		errorMessagesByInvalidValue: new Dictionary<DateTimeOffset, string>
		{
			{ DateTimeOffset.MinValue, "Value must not be the first day of year 0001" }
		});

	NonDefaultDateTimeOffset(DateTimeOffset value) : base(value)
	{
		if (Value is { Year: 1, Month: 1, Day: 1 })
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonDefaultDateTimeOffset>(nameof(Value), value, "@member must not be the first day of year 0001");
		}
	}

	public static NonDefaultDateTimeOffset ApplyConstraintsTo(DateTimeOffset scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonDefaultDateTimeOffset>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonDefaultDateTimeOffset>(defect, scalarValue); }
	}
}
