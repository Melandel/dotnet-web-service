using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class PositiveLong : ConstrainedLong, IConstrainedLong<PositiveLong>
{
	public static ExampleValues<long> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForLongType.Where(i => i >= 0),
		constraintViolationExamples: [ ConstraintViolationExample.Document(-1L, "Value must be equal to or greater than zero") ]);

	PositiveLong(long value) : base(value)
	{
		if (Value < 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<PositiveLong>(nameof(Value), value, "@member must be equal to or greater than zero");
		}
	}

	public static PositiveLong ApplyConstraintsTo(long scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<PositiveLong>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<PositiveLong>(defect, scalarValue); }
	}
}
