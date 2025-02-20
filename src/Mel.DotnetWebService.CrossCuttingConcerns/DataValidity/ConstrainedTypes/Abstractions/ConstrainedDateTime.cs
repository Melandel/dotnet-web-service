namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedDateTime : ConstrainedValue<DateTime>
{
	public ConstrainedDateTime(DateTime value) : base(value)
	{
	}
}
