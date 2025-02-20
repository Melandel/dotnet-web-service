using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

public class NonEmptyGuid : Constrained<Guid>, ICanBeDeserializedFromJson<NonEmptyGuid>
{
	NonEmptyGuid(Guid guid) : base(guid)
	{
		if (Value == Guid.Empty)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(Value), guid, "@member must not be empty");
		}
	}

	public static NonEmptyGuid DeserializeFromJson(string? json)
	=> FromString(json ?? "null");

	public static NonEmptyGuid FromGuid(Guid guid)
	{
		try { return new(guid); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuid>(guid); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuid>(defect, guid); }
	}
	public static NonEmptyGuid FromString(string guidAsString)
	{
		try { return FromGuid(Guid.Parse(guidAsString)); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuid>(guidAsString); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuid>(defect, guidAsString); }
	}
}
