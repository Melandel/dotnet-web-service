namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedFurthermore<TConstrained>
	: ConstrainedType
	where TConstrained : ConstrainedType
{
	protected TConstrained Value;
	protected ConstrainedFurthermore(TConstrained encapsulated)
	{
		Value = encapsulated;
	}

	public static implicit operator TConstrained(ConstrainedFurthermore<TConstrained> constrainedFurthermore) => constrainedFurthermore.Value;
}
