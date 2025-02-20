namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedSByte : ConstrainedValue<sbyte>
{
	public ConstrainedSByte(sbyte value) : base(value)
	{
	}
}
