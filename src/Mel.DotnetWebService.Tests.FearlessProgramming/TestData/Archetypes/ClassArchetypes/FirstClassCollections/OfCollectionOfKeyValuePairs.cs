using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class FirstClassCollectionOfKeyValuePairsType : ConstrainedCollectionOfKeyValuePairs<NonEmptyDictionary<NonZeroInt, NonEmptyString>>, IConstrainedCollectionOfKeyValuePairs<NonZeroInt, NonEmptyString, FirstClassCollectionOfKeyValuePairsType>
	{
		public FirstClassCollectionOfKeyValuePairsType(NonEmptyDictionary<NonZeroInt, NonEmptyString> collectionOfKeyValuePairs) : base(collectionOfKeyValuePairs)
		{ }

		public static ExampleValues<IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ KeyValuePair.Create(Some.Value<NonZeroInt>(), Some.Value<NonEmptyString>()) ],
				[ KeyValuePair.Create(Another.Value<NonZeroInt>(), Another.Value<NonEmptyString>()) ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>>>([ ], "CollectionOfKeyValuePairs must not be empty") ]);

		public static FirstClassCollectionOfKeyValuePairsType ApplyConstraintsTo(IEnumerable<KeyValuePair<NonZeroInt, NonEmptyString>> collectionOfKeyValuePairs)
		{
			try
			{
				return new(NonEmptyDictionary.ApplyConstraintsTo(collectionOfKeyValuePairs));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfKeyValuePairsType>(collectionOfKeyValuePairs);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfKeyValuePairsType>(developerMistake, collectionOfKeyValuePairs);
			}
		}
	}
}

