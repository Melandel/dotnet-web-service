using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyNegativeFloat : ConstrainedFloat, IConstrainedFloat<StrictlyNegativeFloat>
{
	public static ExampleValues<float> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForFloatType.Where(f => f < 0f),
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document(0f           , "Value must be less than zero"),
			ConstraintViolationExample.Document(float.Epsilon, "Value must be less than zero"),
		]);

	StrictlyNegativeFloat(float value) : base(value)
	{
		if (Value >= 0f)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyNegativeFloat>(nameof(Value), value, "@member must be less than zero");
		}
	}

	public static StrictlyNegativeFloat ApplyConstraintsTo(float scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyNegativeFloat>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyNegativeFloat>(defect, scalarValue); }
	}
}
