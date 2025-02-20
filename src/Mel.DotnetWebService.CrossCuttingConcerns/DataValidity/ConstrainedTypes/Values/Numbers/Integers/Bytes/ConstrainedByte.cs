namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedByte : ConstrainedValue<byte>
{
	public ConstrainedByte(byte value) : base(value)
	{
	}
}
