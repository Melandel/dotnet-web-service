using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptySortedList<TKey, TValue> : ConstrainedDictionary<TKey, TValue>, IConstrainedCollectionOfKeyValuePairs<TKey, TValue, NonEmptySortedList<TKey, TValue>> where TKey : notnull
{
	NonEmptySortedList(Dictionary<TKey, TValue> dictionary) : base(dictionary)
	{
		if (CollectionOfKeyValuePairs.Count == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(CollectionOfKeyValuePairs), CollectionOfKeyValuePairs, "@member must not be empty");
		}
	}

	public static ExampleValues<IEnumerable<KeyValuePair<TKey, TValue>>> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues:
		[
			[
				KeyValuePair.Create(Some.ExampleOf<TKey>() ?? Another.ExampleOf<TKey>(), Some.ExampleOf<TValue>())
			],
			[
				KeyValuePair.Create(Some.ExampleOf<TKey>() ?? Another.ExampleOf<TKey>(), Some.ExampleOf<TValue>()),
				KeyValuePair.Create(Some.ExampleOf<TKey>() is null ? YetAnother.ExampleOf<TKey>() : Another.ExampleOf<TKey>(), Another.ExampleOf<TValue>())
			],
		],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<IEnumerable<KeyValuePair<TKey, TValue>>>([ ], "Collection must not be empty"),
		]);

	public static NonEmptySortedList<TKey, TValue> ApplyConstraintsTo(IEnumerable<KeyValuePair<TKey, TValue>> collectionOfKeyValuePairs)
	{
		try { return new(collectionOfKeyValuePairs.ToDictionary()); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptySortedList<TKey, TValue>>(collectionOfKeyValuePairs); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptySortedList<TKey, TValue>>(defect, collectionOfKeyValuePairs); }
	}
}
