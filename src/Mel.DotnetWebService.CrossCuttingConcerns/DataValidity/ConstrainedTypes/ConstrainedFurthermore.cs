namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

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

	public override string? ToString() => Value.ToString();
	public override bool Equals(object? obj) => Value.Equals(obj);
	public bool Equals(TConstrained? other) => Value.Equals(other);

	public override int GetHashCode() => Value.GetHashCode();

	public static bool operator ==(ConstrainedFurthermore<TConstrained>? a, ConstrainedFurthermore<TConstrained>? b)
	=> (a, b) switch
	{
		(null, not null) => false,
		(not null, null) => false,
		(null, null) => true,
		({ Value: var va }, { Value: var vb }) _ => va.Equals(vb)
	};
	public static bool operator !=(ConstrainedFurthermore<TConstrained>? a, ConstrainedFurthermore<TConstrained>? b) => !(a != b);

	public static bool operator ==(ConstrainedFurthermore<TConstrained>? a, TConstrained? b)
	=> (a, b) switch
	{
		(null, not null) => false,
		(not null, null) => false,
		(null, null) => true,
		({ Value: var va }, var vb) _ => va.Equals(vb)
	};
	public static bool operator !=(ConstrainedFurthermore<TConstrained>? a, TConstrained? b) => !(a != b);

	public static bool operator ==(TConstrained? a, ConstrainedFurthermore<TConstrained>? b)
	=> b == a;
	public static bool operator !=(TConstrained? a, ConstrainedFurthermore<TConstrained>? b) => !(a != b);
}
