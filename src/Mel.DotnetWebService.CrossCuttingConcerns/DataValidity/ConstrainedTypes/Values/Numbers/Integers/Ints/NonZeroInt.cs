using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroInt : ConstrainedInt, IConstrainedInt<NonZeroInt>
{
	public static ExampleValues<int> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForIntType.Where(i => i != default),
		constraintViolationExamples: [ ConstraintViolationExample.Document(0, "Value must not be zero") ]);

	NonZeroInt(int value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroInt>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroInt ApplyConstraintsTo(int scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroInt>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroInt>(defect, scalarValue); }
	}
}
