using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyString : ConstrainedString, IConstrainedString<NonEmptyString>
{
	public static ExampleValues<string> Examples
	=>	ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForStringType.Where(x => x != ""),
		constraintViolationExamples: [ ConstraintViolationExample.Document("", "Value must not be empty") ]);

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
