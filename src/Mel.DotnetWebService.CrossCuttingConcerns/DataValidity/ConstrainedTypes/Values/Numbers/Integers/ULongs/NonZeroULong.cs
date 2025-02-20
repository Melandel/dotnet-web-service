using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroULong : ConstrainedULong, IConstrainedULong<NonZeroULong>
{
	public static ExampleValues<ulong> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForULongType.Where(ul => ul != default),
		constraintViolationExamples: [ ConstraintViolationExample.Document<ulong>(0, "Value must not be zero") ]);

	NonZeroULong(ulong value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroULong>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroULong ApplyConstraintsTo(ulong scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroULong>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroULong>(defect, scalarValue); }
	}
}
