namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;

public interface ICanBeDeserializedFromJson<TTypeInsideJson, TDeserialized>
{
	public abstract static TDeserialized ReconstituteFrom(TTypeInsideJson valueFoundInsideJson);
}
