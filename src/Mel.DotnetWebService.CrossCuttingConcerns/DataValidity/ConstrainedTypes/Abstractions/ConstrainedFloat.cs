namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedFloat : ConstrainedValue<float>
{
	public ConstrainedFloat(float value) : base(value)
	{
	}
}
