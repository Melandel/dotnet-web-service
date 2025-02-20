namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedDouble : ConstrainedValue<double>
{
	public ConstrainedDouble(double value) : base(value)
	{
	}
}
