using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class FirstClassCollectionOfArraysConstructedFromIEnumerableType : ConstrainedCollection<NonEmptyGuid[][]>, IConstrainedCollection<IEnumerable<Guid>, FirstClassCollectionOfArraysConstructedFromIEnumerableType>
	{
		FirstClassCollectionOfArraysConstructedFromIEnumerableType(NonEmptyGuid[][] value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<IEnumerable<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<IEnumerable<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfArraysConstructedFromIEnumerableType ApplyConstraintsTo(IEnumerable<IEnumerable<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfArraysConstructedFromIEnumerableType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfArraysConstructedFromIEnumerableType>(developerMistake, collection);
			}
		}
	}
}
