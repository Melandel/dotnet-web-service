namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;

public abstract class Constrained<TValue>
{
	protected TValue Value;
	protected Constrained(TValue value)
	{
		Value = value;
	}

	public static implicit operator TValue(Constrained<TValue> constrainedValue) => constrainedValue.Value;
	public override string? ToString() => Value?.ToString();
}
