namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedShort : ConstrainedValue<short>
{
	public ConstrainedShort(short value) : base(value)
	{
	}
}
