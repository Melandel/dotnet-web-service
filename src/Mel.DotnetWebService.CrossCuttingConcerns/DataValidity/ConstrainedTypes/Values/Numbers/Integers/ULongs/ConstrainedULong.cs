namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedULong : ConstrainedValue<ulong>
{
	public ConstrainedULong(ulong value) : base(value)
	{
	}
}
