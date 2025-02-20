using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class PositiveInt : ConstrainedInt, IConstrainedInt<PositiveInt>
{
	public static ExampleValues<int> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForIntType.Where(i => i >= 0),
		constraintViolationExamples: [ ConstraintViolationExample.Document(-1, "Value must be equal to or greater than zero") ]);

	PositiveInt(int value) : base(value)
	{
		if (Value < 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<PositiveInt>(nameof(Value), value, "@member must be equal to or greater than zero");
		}
	}

	public static PositiveInt ApplyConstraintsTo(int scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<PositiveInt>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<PositiveInt>(defect, scalarValue); }
	}
}
