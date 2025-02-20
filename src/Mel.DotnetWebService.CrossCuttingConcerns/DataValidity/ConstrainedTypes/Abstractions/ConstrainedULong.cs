namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedULong : ConstrainedValue<ulong>
{
	public ConstrainedULong(ulong value) : base(value)
	{
	}
}
