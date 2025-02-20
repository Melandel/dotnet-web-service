using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class FirstClassCollectionOfValuesType : ConstrainedCollection<NonEmptyGuid[]>, IConstrainedCollection<Guid, FirstClassCollectionOfValuesType>
	{
		FirstClassCollectionOfValuesType(NonEmptyGuid[] value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ Some.Value<NonEmptyGuid>() ],
				[ Another.Value<NonEmptyGuid>(), YetAnother.Value<NonEmptyGuid>() ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid>>([ Guid.Empty ], "Value must not be empty") ]);

		public static FirstClassCollectionOfValuesType ApplyConstraintsTo(IEnumerable<Guid> collection)
		{
			try
			{
				return new(collection.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<FirstClassCollectionOfValuesType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<FirstClassCollectionOfValuesType>(developerMistake, collection);
			}
		}
	}
}
