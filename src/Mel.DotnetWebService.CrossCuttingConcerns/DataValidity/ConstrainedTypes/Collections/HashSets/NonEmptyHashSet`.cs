namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyHashSet<T> : ConstrainedHashSet<T>, IConstrainedCollection<T, NonEmptyHashSet<T>>
{
	NonEmptyHashSet(HashSet<T> hashset) : base(hashset)
	{
		if (!Collection.Any())
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(Collection), Collection, "@member must not be empty");
		}
	}

	public static NonEmptyHashSet<T> ApplyConstraintsTo(IEnumerable<T> collection)
	{
		try { return new(collection.ToHashSet()); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, collection); }
	}
}
