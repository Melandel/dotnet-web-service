using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class NonGenericConstrainedCollectionOfValuesType : ConstrainedCollection<NonEmptyGuid[]>, IConstrainedCollection<Guid, NonGenericConstrainedCollectionOfValuesType>
	{
		NonGenericConstrainedCollectionOfValuesType(NonEmptyGuid[] value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ Some.Value<NonEmptyGuid>() ],
				[ Another.Value<NonEmptyGuid>(), YetAnother.Value<NonEmptyGuid>() ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid>>([ Guid.Empty ], "Value must not be empty") ]);

		public static NonGenericConstrainedCollectionOfValuesType ApplyConstraintsTo(IEnumerable<Guid> collection)
		{
			try
			{
				return new(collection.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonGenericConstrainedCollectionOfValuesType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonGenericConstrainedCollectionOfValuesType>(developerMistake, collection);
			}
		}
	}
}
