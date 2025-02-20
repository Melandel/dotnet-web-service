namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedDateTime : ConstrainedValue<DateTime>
{
	public ConstrainedDateTime(DateTime value) : base(value)
	{
	}
}
