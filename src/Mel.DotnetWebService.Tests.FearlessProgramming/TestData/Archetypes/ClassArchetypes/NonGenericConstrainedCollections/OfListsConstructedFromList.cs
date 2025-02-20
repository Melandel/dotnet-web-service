using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class NonGenericConstrainedCollectionOfListsConstructedFromListType : ConstrainedCollection<List<List<NonEmptyGuid>>>, IConstrainedCollection<List<Guid>, NonGenericConstrainedCollectionOfListsConstructedFromListType>
	{
		NonGenericConstrainedCollectionOfListsConstructedFromListType(List<List<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<List<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<List<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static NonGenericConstrainedCollectionOfListsConstructedFromListType ApplyConstraintsTo(IEnumerable<List<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToList()).ToList());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonGenericConstrainedCollectionOfListsConstructedFromListType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonGenericConstrainedCollectionOfListsConstructedFromListType>(developerMistake, collection);
			}
		}
	}
}
