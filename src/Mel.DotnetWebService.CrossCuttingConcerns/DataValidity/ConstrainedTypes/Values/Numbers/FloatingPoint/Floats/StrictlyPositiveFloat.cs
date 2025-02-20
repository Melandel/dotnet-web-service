using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyPositiveFloat : ConstrainedFloat, IConstrainedFloat<StrictlyPositiveFloat>
{
	public static ExampleValues<float> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForFloatType.Where(f => f > 0f),
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document(0f            , "Value must be greater than zero"),
			ConstraintViolationExample.Document(-float.Epsilon, "Value must be greater than zero"),
		]);

	StrictlyPositiveFloat(float value) : base(value)
	{
		if (Value <= 0f)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyPositiveFloat>(nameof(Value), value, "@member must be greater than zero");
		}
	}

	public static StrictlyPositiveFloat ApplyConstraintsTo(float scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyPositiveFloat>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyPositiveFloat>(defect, scalarValue); }
	}
}
