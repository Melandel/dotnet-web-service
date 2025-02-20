using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyPositiveShort : ConstrainedShort, IConstrainedShort<StrictlyPositiveShort>
{
	public static ExampleValues<short> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForShortType.Where(s => s > 0),
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<short>(-1, "Value must be greater than zero"),
			ConstraintViolationExample.Document<short>( 0, "Value must be greater than zero"),
		]);

	StrictlyPositiveShort(short value) : base(value)
	{
		if (Value <= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyPositiveShort>(nameof(Value), value, "@member must be greater than zero");
		}
	}

	public static StrictlyPositiveShort ApplyConstraintsTo(short scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyPositiveShort>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyPositiveShort>(defect, scalarValue); }
	}
}
