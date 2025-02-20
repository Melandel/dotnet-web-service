using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NegativeSByte : ConstrainedSByte, IConstrainedSByte<NegativeSByte>
{
	public static ExampleValues<sbyte> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForSByteType.Where(sb => sb <= 0),
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<sbyte>(1, "Value must be equal to or less than 0"),
		]);

	NegativeSByte(sbyte value) : base(value)
	{
		if (Value > 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NegativeSByte>(nameof(Value), value, "@member must be equal to or less than 0");
		}
	}

	public static NegativeSByte ApplyConstraintsTo(sbyte scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NegativeSByte>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NegativeSByte>(defect, scalarValue); }
	}
}
