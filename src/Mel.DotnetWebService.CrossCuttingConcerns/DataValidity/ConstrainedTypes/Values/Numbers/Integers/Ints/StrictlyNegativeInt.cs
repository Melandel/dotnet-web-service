using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyNegativeInt : ConstrainedInt, IConstrainedInt<StrictlyNegativeInt>
{
	public static ExampleValues<int> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: [ -24, -60, -120 ],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document(0, "Value must be less than zero"),
			ConstraintViolationExample.Document(1, "Value must be less than zero"),
		]);

	StrictlyNegativeInt(int value) : base(value)
	{
		if (Value >= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyNegativeInt>(nameof(Value), value, "@member must be less than zero");
		}
	}

	public static StrictlyNegativeInt ApplyConstraintsTo(int scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyNegativeInt>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyNegativeInt>(defect, scalarValue); }
	}
}
