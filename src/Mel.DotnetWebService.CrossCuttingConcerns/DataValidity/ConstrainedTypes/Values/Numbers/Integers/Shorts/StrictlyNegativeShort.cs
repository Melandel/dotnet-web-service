using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyNegativeShort : ConstrainedShort, IConstrainedShort<StrictlyNegativeShort>
{
	public static ExampleValues<short> Examples
	=> ExampleValues.ValidAndInvalid<short>(
		validValues: [ -24, -60, -120 ],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<short>(1, "Value must be less than zero"),
			ConstraintViolationExample.Document<short>(0, "Value must be less than zero"),
		]);

	StrictlyNegativeShort(short value) : base(value)
	{
		if (Value >= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyNegativeShort>(nameof(Value), value, "@member must be less than zero");
		}
	}

	public static StrictlyNegativeShort ApplyConstraintsTo(short scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyNegativeShort>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyNegativeShort>(defect, scalarValue); }
	}
}
