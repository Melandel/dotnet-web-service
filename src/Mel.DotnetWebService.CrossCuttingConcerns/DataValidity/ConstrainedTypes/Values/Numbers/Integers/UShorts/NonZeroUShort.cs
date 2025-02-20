using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroUShort : ConstrainedUShort, IConstrainedUShort<NonZeroUShort>
{
	public static ExampleValues<ushort> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForUShortType.Where(us => us != default),
		constraintViolationExamples: [ ConstraintViolationExample.Document<ushort>(0, "Value must not be zero") ]);

	NonZeroUShort(ushort value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroUShort>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroUShort ApplyConstraintsTo(ushort scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroUShort>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroUShort>(defect, scalarValue); }
	}
}
