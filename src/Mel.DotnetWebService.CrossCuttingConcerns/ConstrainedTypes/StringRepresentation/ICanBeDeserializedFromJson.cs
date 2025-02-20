namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

public interface ICanBeDeserializedFromJson<T>
{
	public abstract static T? DeserializeFromJson(string? json);
}
