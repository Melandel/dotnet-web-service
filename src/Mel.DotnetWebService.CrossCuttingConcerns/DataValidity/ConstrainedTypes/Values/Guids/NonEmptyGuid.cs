using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyGuid : ConstrainedGuid, IConstrainedValue<Guid, NonEmptyGuid>
{
	public static ExampleValues<Guid> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForGuidType.Where(x => x != default),
		constraintViolationExamples: [ ConstraintViolationExample.Document(Guid.Empty, "Value must not be empty") ]);

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
	public static NonEmptyGuid ApplyConstraintsTo(string guidAsString)
	{
		try { return ApplyConstraintsTo(Guid.Parse(guidAsString)); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuid>(guidAsString); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuid>(defect, guidAsString); }
	}
}
