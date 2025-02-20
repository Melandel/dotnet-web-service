using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyList<T> : ConstrainedList<T>, IConstrainedCollection<T, NonEmptyList<T>>
{
	NonEmptyList(List<T> list) : base(list)
	{
		if (Collection.Count == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(Collection), Collection, "@member must not be empty");
		}
	}

	public static ExampleValues<IEnumerable<T>> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ValidExampleValues.ForDuplicatesFriendlyCollectionTypes<T>(),
		constraintViolationExamples: new[]
		{
			ConstraintViolationExample.Document<IEnumerable<T>>(
				[],
				"Collection must not be empty"),
		});

	public static NonEmptyList<T> ApplyConstraintsTo(IEnumerable<T> collection)
	{
		try { return new(collection.ToList()); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyList<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyList<T>>(defect, collection); }
	}
}
