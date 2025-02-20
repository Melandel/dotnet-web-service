namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedUShort : ConstrainedValue<ushort>
{
	public ConstrainedUShort(ushort value) : base(value)
	{
	}
}
