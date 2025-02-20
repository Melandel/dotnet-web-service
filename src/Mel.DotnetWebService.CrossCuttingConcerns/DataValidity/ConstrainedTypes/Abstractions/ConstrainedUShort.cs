namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedUShort : ConstrainedValue<ushort>
{
	public ConstrainedUShort(ushort value) : base(value)
	{
	}
}
