using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NegativeFloat : ConstrainedFloat, IConstrainedFloat<NegativeFloat>
{
	public static ExampleValues<float> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForFloatType.Where(f => f <= 0f),
		constraintViolationExamples: [ ConstraintViolationExample.Document(float.Epsilon, "Value must be equal to or less than zero") ]);

	NegativeFloat(float value) : base(value)
	{
		if (Value > 0f)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NegativeFloat>(nameof(Value), value, "@member must be equal to or less than zero");
		}
	}

	public static NegativeFloat ApplyConstraintsTo(float scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NegativeFloat>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NegativeFloat>(defect, scalarValue); }
	}
}
