namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedGuid : ConstrainedValue<Guid>
{
	public ConstrainedGuid(Guid value) : base(value)
	{
	}
}
