using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class FirstClassCollectionOfIReadOnlyCollectionsType : ConstrainedCollection<IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>>>, IConstrainedCollection<Guid[], FirstClassCollectionOfIReadOnlyCollectionsType>
	{
		FirstClassCollectionOfIReadOnlyCollectionsType(IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid[]>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid[]>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfIReadOnlyCollectionsType ApplyConstraintsTo(IEnumerable<Guid[]> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfIReadOnlyCollectionsType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfIReadOnlyCollectionsType>(developerMistake, collection);
			}
		}
	}
}
