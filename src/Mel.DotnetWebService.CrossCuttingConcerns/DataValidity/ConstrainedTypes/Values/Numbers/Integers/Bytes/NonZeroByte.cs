using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroByte : ConstrainedByte, IConstrainedByte<NonZeroByte>
{
	public static ExampleValues<byte> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForByteType.Where(b => b!= default),
		constraintViolationExamples: [ ConstraintViolationExample.Document<byte>(0, "Value must not be zero") ]);

	NonZeroByte(byte value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroByte>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroByte ApplyConstraintsTo(byte scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroByte>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroByte>(defect, scalarValue); }
	}
}
