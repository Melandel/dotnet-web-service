using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class PositiveFloat : ConstrainedFloat, IConstrainedFloat<PositiveFloat>
{
	public static ExampleValues<float> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForFloatType.Where(f => f >= 0f),
		constraintViolationExamples: [ ConstraintViolationExample.Document(-float.Epsilon, "Value must be equal to or greater than zero") ]);

	PositiveFloat(float value) : base(value)
	{
		if (Value < 0f)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<PositiveFloat>(nameof(Value), value, "@member must be equal to or greater than zero");
		}
	}

	public static PositiveFloat ApplyConstraintsTo(float scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<PositiveFloat>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<PositiveFloat>(defect, scalarValue); }
	}
}
