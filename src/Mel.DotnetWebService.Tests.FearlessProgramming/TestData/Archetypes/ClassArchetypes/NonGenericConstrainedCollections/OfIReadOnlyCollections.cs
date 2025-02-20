using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class NonGenericConstrainedCollectionOfIReadOnlyCollectionsType : ConstrainedCollection<IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>>>, IConstrainedCollection<Guid[], NonGenericConstrainedCollectionOfIReadOnlyCollectionsType>
	{
		NonGenericConstrainedCollectionOfIReadOnlyCollectionsType(IReadOnlyCollection<IReadOnlyCollection<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid[]>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues: ValidExampleValues.ForDuplicatesFriendlyCollectionTypes<List<NonEmptyGuid>>()
				.Select(x => x.Select(y => y.Select(z => (Guid)z).ToArray())),
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid[]>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static NonGenericConstrainedCollectionOfIReadOnlyCollectionsType ApplyConstraintsTo(IEnumerable<Guid[]> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonGenericConstrainedCollectionOfIReadOnlyCollectionsType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonGenericConstrainedCollectionOfIReadOnlyCollectionsType>(developerMistake, collection);
			}
		}
	}
}
