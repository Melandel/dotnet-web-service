using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class StrictlyPositiveSByte : ConstrainedSByte, IConstrainedSByte<StrictlyPositiveSByte>
{
	public static ExampleValues<sbyte> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForSByteType.Where(sb => sb > 0),
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<sbyte>( 0, "Value must be greater than 0"),
			ConstraintViolationExample.Document<sbyte>(-1, "Value must be greater than 0"),
		]);

	StrictlyPositiveSByte(sbyte value) : base(value)
	{
		if (Value <= 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<StrictlyPositiveSByte>(nameof(Value), value, "@member must be greater than 0");
		}
	}

	public static StrictlyPositiveSByte ApplyConstraintsTo(sbyte scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<StrictlyPositiveSByte>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<StrictlyPositiveSByte>(defect, scalarValue); }
	}
}
