namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedInt : ConstrainedValue<int>
{
	public ConstrainedInt(int value) : base(value)
	{
	}
}
