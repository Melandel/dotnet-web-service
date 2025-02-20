using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class NonGenericConstrainedCollectionOfKeyValuePairsType : ConstrainedCollectionOfKeyValuePairs<NonEmptyDictionary<NonZeroInt, NonEmptyString>>, IConstrainedCollectionOfKeyValuePairs<NonZeroInt, NonEmptyString, NonGenericConstrainedCollectionOfKeyValuePairsType>
	{
		public NonGenericConstrainedCollectionOfKeyValuePairsType(NonEmptyDictionary<NonZeroInt, NonEmptyString> collectionOfKeyValuePairs) : base(collectionOfKeyValuePairs)
		{ }

		public static ExampleValues<IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ KeyValuePair.Create(Some.Value<NonZeroInt>(), Some.Value<NonEmptyString>()) ],
				[ KeyValuePair.Create(Another.Value<NonZeroInt>(), Another.Value<NonEmptyString>()) ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>>>([ ], "CollectionOfKeyValuePairs must not be empty") ]);

		public static NonGenericConstrainedCollectionOfKeyValuePairsType ApplyConstraintsTo(IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>> collectionOfKeyValuePairs)
		{
			try
			{
				return new(NonEmptyDictionary.ApplyConstraintsTo(collectionOfKeyValuePairs));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonGenericConstrainedCollectionOfKeyValuePairsType>(collectionOfKeyValuePairs);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonGenericConstrainedCollectionOfKeyValuePairsType>(developerMistake, collectionOfKeyValuePairs);
			}
		}
	}
}

