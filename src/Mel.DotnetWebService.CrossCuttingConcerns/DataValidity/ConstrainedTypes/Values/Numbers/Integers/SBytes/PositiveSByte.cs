using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class PositiveSByte : ConstrainedSByte, IConstrainedSByte<PositiveSByte>
{
	public static ExampleValues<sbyte> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForSByteType.Where(sb => sb >= 0),
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<sbyte>(-1, "Value must be equal to or greater than 0"),
		]);

	PositiveSByte(sbyte value) : base(value)
	{
		if (Value < 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<PositiveSByte>(nameof(Value), value, "@member must be equal to or greater than 0");
		}
	}

	public static PositiveSByte ApplyConstraintsTo(sbyte scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<PositiveSByte>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<PositiveSByte>(defect, scalarValue); }
	}
}
