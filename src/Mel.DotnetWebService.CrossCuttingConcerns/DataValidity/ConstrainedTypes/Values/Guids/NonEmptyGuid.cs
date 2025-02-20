namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyGuid : ConstrainedGuid, IConstrainedValue<Guid, NonEmptyGuid>
{
	public static ExampleValues<Guid> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new[]
		{
			Guid.Parse("00000000-0000-0000-0000-000000000001"),
			Guid.Parse("00000000-0000-0000-0000-00000000000a"),
			Guid.Parse("a7ce1ce2-82fc-4bea-82cc-d27f47e527ed")
		},
		errorMessagesByInvalidValue: new Dictionary<Guid, string>
		{
			{ Guid.Empty, "Value must not be empty" }
		});

	NonEmptyGuid(Guid value) : base(value)
	{
		if (Value == Guid.Empty)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(Value), value, "@member must not be empty");
		}
	}

	public static NonEmptyGuid ApplyConstraintsTo(Guid scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuid>(scalarValue); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuid>(defect, scalarValue); }
	}
	public static NonEmptyGuid Constraining(string guidAsString)
	{
		try { return ApplyConstraintsTo(Guid.Parse(guidAsString)); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuid>(guidAsString); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuid>(defect, guidAsString); }
	}
}
