namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedUInt : ConstrainedValue<uint>
{
	public ConstrainedUInt(uint value) : base(value)
	{
	}
}
