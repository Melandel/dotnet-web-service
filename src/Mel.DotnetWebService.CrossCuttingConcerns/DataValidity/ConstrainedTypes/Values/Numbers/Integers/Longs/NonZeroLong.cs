using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroLong : ConstrainedLong, IConstrainedLong<NonZeroLong>
{
	public static ExampleValues<long> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForLongType.Where(l => l != default),
		constraintViolationExamples: [ ConstraintViolationExample.Document(0L, "Value must not be zero") ]);

	NonZeroLong(long value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroLong>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroLong ApplyConstraintsTo(long scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroLong>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroLong>(defect, scalarValue); }
	}
}
