namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedFloat : ConstrainedValue<float>
{
	public ConstrainedFloat(float value) : base(value)
	{
	}
}
