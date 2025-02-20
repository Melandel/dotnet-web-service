namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

public interface ICanBeDeserializedFromJson<TTypeInsideJson, TDeserialized>
{
	public abstract static TDeserialized ReconstituteFrom(TTypeInsideJson valueFoundInsideJson);
}
