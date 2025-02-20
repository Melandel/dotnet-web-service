using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids;

public class NonEmptyGuid : Constrained<Guid>, ICanBeDeserializedFromJson<Guid, NonEmptyGuid>
{
	protected NonEmptyGuid(Guid guid) : base(guid)
	{
		if (Value == Guid.Empty)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(Value), guid, "@member must not be empty");
		}
	}

	public static NonEmptyGuid ReconstituteFrom(Guid valueFoundInsideJson)
	=> FromGuid(valueFoundInsideJson);

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
