namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedGuid : ConstrainedValue<Guid>
{
	public ConstrainedGuid(Guid value) : base(value)
	{
	}
}
