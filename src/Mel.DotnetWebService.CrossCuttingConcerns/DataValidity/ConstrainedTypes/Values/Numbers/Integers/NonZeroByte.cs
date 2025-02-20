using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Core;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroByte : ConstrainedByte, IConstrainedByte<NonZeroByte>
{
	public static ExampleValues<byte> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new byte[] { 1, 2, 3, 255 },
		errorMessagesByInvalidValue: new Dictionary<byte, string>
		{
			{ 0, "Value must not be zero" }
		});

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
