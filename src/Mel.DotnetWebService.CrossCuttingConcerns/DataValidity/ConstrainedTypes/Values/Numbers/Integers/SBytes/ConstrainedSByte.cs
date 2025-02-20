namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedSByte : ConstrainedValue<sbyte>
{
	public ConstrainedSByte(sbyte value) : base(value)
	{
	}
}
