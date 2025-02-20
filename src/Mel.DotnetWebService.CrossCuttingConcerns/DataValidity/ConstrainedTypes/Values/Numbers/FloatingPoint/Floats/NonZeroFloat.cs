using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroFloat : ConstrainedFloat, IConstrainedFloat<NonZeroFloat>
{
	public static ExampleValues<float> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForFloatType.Where(f => f != default),
		constraintViolationExamples: [ ConstraintViolationExample.Document(0f, "Value must not be zero") ]);

	NonZeroFloat(float value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroFloat>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroFloat ApplyConstraintsTo(float scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroFloat>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroFloat>(defect, scalarValue); }
	}
}
