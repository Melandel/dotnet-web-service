namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;

public class NonEmptyGuid : EncapsulationOf<Guid>
{
	NonEmptyGuid(Guid guid) : base(guid)
	{
		if (Encapsulated == Guid.Empty)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(Encapsulated), guid, "@member must not be empty");
		}
	}

	public static NonEmptyGuid FromGuid(Guid guid)
	{
		try
		{
			return new(guid);
		}
		catch (ObjectConstructionException objectConstructionException)
		{
			objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuid>(guid);
			throw;
		}
		catch (Exception developerMistake)
		{
			throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuid>(developerMistake, guid);
		}
	}
	public static NonEmptyGuid FromString(string guidAsString)
	{
		try
		{
			return FromGuid(Guid.Parse(guidAsString));
		}
		catch (ObjectConstructionException objectConstructionException)
		{
			objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuid>(guidAsString);
			throw;
		}
		catch (Exception developerMistake)
		{
			throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuid>(developerMistake, guidAsString);
		}
	}
}
