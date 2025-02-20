using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyPositiveInt : ConstrainedInt, IConstrainedInt<StrictlyPositiveInt>
{
	public static ExampleValues<int> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForIntType.Where(i => i > 0),
		constraintViolationExamples: [ ConstraintViolationExample.Document(-1, "Value must be greater than zero") ]);

	StrictlyPositiveInt(int value) : base(value)
	{
		if (Value <= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyPositiveInt>(nameof(Value), value, "@member must be greater than zero");
		}
	}

	public static StrictlyPositiveInt ApplyConstraintsTo(int scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyPositiveInt>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyPositiveInt>(defect, scalarValue); }
	}
}
