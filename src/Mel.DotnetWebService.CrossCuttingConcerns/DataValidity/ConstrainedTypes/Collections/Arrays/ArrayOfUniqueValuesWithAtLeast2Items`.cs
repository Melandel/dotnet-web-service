namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class ArrayOfUniqueValuesWithAtLeast2Items<T> : ConstrainedArray<T>, IConstrainedCollection<T, ArrayOfUniqueValuesWithAtLeast2Items<T>>
{
	ArrayOfUniqueValuesWithAtLeast2Items(T[] value) : base(value)
	{
		if (Collection.Length < 2)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<ArrayOfUniqueValuesWithAtLeast2Items<T>>(nameof(value), value, $"@member must have at least 2 elements, but instead has {Collection.Length} elements");
		}

		if (Collection.Distinct().Count() != Collection.Length)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<ArrayOfUniqueValuesWithAtLeast2Items<T>>(nameof(value), value, $"@member must have no duplicate elements");
		}
	}

	public static ArrayOfUniqueValuesWithAtLeast2Items<T> ApplyConstraintsTo(IEnumerable<T> collection)
	{
		try { return new(collection.ToArray()); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, collection); }
	}

	public static ArrayOfUniqueValuesWithAtLeast2Items<T> ApplyConstraintsTo(IEnumerable<T[]> collection)
	{
		throw new NotImplementedException();
	}
}
