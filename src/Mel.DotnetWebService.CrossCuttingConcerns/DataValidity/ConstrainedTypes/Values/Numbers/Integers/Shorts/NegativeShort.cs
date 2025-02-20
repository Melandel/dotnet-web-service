using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NegativeShort : ConstrainedShort, IConstrainedShort<NegativeShort>
{
	public static ExampleValues<short> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForShortType.Where(s => s <= 0),
		constraintViolationExamples: [ ConstraintViolationExample.Document<short>(1, "Value must be equal to or less than zero") ]);

	NegativeShort(short value) : base(value)
	{
		if (Value > 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NegativeShort>(nameof(Value), value, "@member must be equal to or less than zero");
		}
	}

	public static NegativeShort ApplyConstraintsTo(short scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NegativeShort>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NegativeShort>(defect, scalarValue); }
	}
}
