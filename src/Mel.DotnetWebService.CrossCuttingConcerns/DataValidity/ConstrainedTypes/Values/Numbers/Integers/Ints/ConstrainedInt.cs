namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedInt : ConstrainedValue<int>
{
	public ConstrainedInt(int value) : base(value)
	{
	}
}
