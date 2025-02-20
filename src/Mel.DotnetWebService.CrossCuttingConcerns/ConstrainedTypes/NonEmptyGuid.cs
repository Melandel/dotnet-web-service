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
			var _ when guid == Guid.Empty => throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(_encapsulated), guid),
			_ => guid
		};
	}

	public static NonEmptyGuid From(Guid guid)
	{
		try
		{
			return new(guid);
		}
		catch (ObjectConstructionException objectConstructionException)
		{
			objectConstructionException.EnrichWithInformationAbout<NonEmptyGuid>(guid);
			throw;
		}
		catch (Exception developerMistake)
		{
			throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuid>(developerMistake, guid);
		}
	}
}
