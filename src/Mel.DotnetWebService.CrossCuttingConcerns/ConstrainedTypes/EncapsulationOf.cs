namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;

public abstract class EncapsulationOf<T>
{
	protected T Encapsulated;
	protected EncapsulationOf(T encapsulated)
	{
		Encapsulated = encapsulated;
	}

	public static implicit operator T(EncapsulationOf<T> enumValue) => enumValue.Encapsulated;
	public override string? ToString() => $"{Encapsulated}";
}
