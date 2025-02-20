using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class FirstClassCollectionOfListsConstructedFromListType : ConstrainedCollection<List<List<NonEmptyGuid>>>, IConstrainedCollection<List<Guid>, FirstClassCollectionOfListsConstructedFromListType>
	{
		FirstClassCollectionOfListsConstructedFromListType(List<List<NonEmptyGuid>> value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<List<Guid>>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ [ Some.Value<NonEmptyGuid>() ] ],
				[ [Another.Value<NonEmptyGuid>() ], [ YetAnother.Value<NonEmptyGuid>() ] ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<List<Guid>>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static FirstClassCollectionOfListsConstructedFromListType ApplyConstraintsTo(IEnumerable<List<Guid>> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToList()).ToList());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfListsConstructedFromListType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfListsConstructedFromListType>(developerMistake, collection);
			}
		}
	}
}
