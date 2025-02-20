using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NegativeInt : ConstrainedInt, IConstrainedInt<NegativeInt>
{
	public static ExampleValues<int> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForIntType.Where(i => i <= 0),
		constraintViolationExamples: [ ConstraintViolationExample.Document(1, "Value must be equal to or less than zero") ]);

	NegativeInt(int value) : base(value)
	{
		if (Value > 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NegativeInt>(nameof(Value), value, "@member must be equal to or less than zero");
		}
	}

	public static NegativeInt ApplyConstraintsTo(int scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NegativeInt>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NegativeInt>(defect, scalarValue); }
	}
}
