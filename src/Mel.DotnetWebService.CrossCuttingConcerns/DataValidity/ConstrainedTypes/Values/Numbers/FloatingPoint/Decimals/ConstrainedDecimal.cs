namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedDecimal : ConstrainedValue<decimal>
{
	public ConstrainedDecimal(decimal value) : base(value)
	{
	}
}
