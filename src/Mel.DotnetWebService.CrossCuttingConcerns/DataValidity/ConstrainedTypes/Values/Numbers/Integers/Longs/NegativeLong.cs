using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NegativeLong : ConstrainedLong, IConstrainedLong<NegativeLong>
{
	public static ExampleValues<long> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForLongType.Where(i => i <= 0),
		constraintViolationExamples: [ ConstraintViolationExample.Document(1L, "Value must be equal to or less than zero") ]);

	NegativeLong(long value) : base(value)
	{
		if (Value > 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NegativeLong>(nameof(Value), value, "@member must be equal to or less than zero");
		}
	}

	public static NegativeLong ApplyConstraintsTo(long scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NegativeLong>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NegativeLong>(defect, scalarValue); }
	}
}
