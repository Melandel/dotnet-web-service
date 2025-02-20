namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedDateTimeOffset : ConstrainedValue<DateTimeOffset>
{
	public ConstrainedDateTimeOffset(DateTimeOffset value) : base(value)
	{
	}
}
