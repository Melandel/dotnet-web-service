namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedShort : ConstrainedValue<short>
{
	public ConstrainedShort(short value) : base(value)
	{
	}
}
