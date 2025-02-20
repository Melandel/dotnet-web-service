using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyNegativeLong : ConstrainedLong, IConstrainedLong<StrictlyNegativeLong>
{
	public static ExampleValues<long> Examples
	=> ExampleValues.ValidAndInvalid<long>(
		validValues: [ -24, -60, -120 ],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document(0L, "Value must be less than zero"),
			ConstraintViolationExample.Document(1L, "Value must be less than zero"),
		]);

	StrictlyNegativeLong(long value) : base(value)
	{
		if (Value >= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyNegativeLong>(nameof(Value), value, "@member must be less than zero");
		}
	}

	public static StrictlyNegativeLong ApplyConstraintsTo(long scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyNegativeLong>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyNegativeLong>(defect, scalarValue); }
	}
}
