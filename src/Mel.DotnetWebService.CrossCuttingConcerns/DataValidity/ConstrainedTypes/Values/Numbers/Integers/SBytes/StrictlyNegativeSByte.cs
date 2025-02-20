using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyNegativeSByte : ConstrainedSByte, IConstrainedSByte<StrictlyNegativeSByte>
{
	public static ExampleValues<sbyte> Examples
	=> ExampleValues.ValidAndInvalid<sbyte>(
		validValues: [ -24, -60, -120 ],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<sbyte>(0, "Value must be less than 0"),
			ConstraintViolationExample.Document<sbyte>(1, "Value must be less than 0"),
		]);

	StrictlyNegativeSByte(sbyte value) : base(value)
	{
		if (Value >= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyNegativeSByte>(nameof(Value), value, "@member must be less than 0");
		}
	}

	public static StrictlyNegativeSByte ApplyConstraintsTo(sbyte scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyNegativeSByte>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyNegativeSByte>(defect, scalarValue); }
	}
}
