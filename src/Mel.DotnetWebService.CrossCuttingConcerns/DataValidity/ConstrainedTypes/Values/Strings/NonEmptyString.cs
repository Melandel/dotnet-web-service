namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Strings;

public sealed class NonEmptyString : ConstrainedString, IConstrainedString<NonEmptyString>
{
	public static ExampleValues<string> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new[] { "foo", "bar", "1", "\"", " " },
		errorMessagesByInvalidValue: new Dictionary<string, string> {
			{ "", "Value must not be empty" }
		});

	NonEmptyString(string value) : base(value)
	{
		if (Value == "")
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyString>(nameof(Value), value, "@member must not be empty");
		}
	}

	public static NonEmptyString ApplyConstraintsTo(string scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyString>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyString>(defect, scalarValue); }
	}
}
