namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;

public class NonEmptyGuid
{
	public override string ToString() => $"{_encapsulated}";
	public static implicit operator Guid(NonEmptyGuid obj) => obj._encapsulated;

	readonly Guid _encapsulated;
	NonEmptyGuid(Guid guid)
	{
		_encapsulated = guid switch
		{
			var _ when guid == Guid.Empty => throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(_encapsulated), guid, "@member must not be empty"),
			_ => guid
		};
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
