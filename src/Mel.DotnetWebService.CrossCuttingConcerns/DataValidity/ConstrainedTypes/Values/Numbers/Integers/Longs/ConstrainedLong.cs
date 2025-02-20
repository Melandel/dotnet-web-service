namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedLong : ConstrainedValue<long>
{
	public ConstrainedLong(long value) : base(value)
	{
	}
}
