namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedByte : ConstrainedValue<byte>
{
	public ConstrainedByte(byte value) : base(value)
	{
	}
}
