using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json.Linq;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public sealed class NonGenericConstrainedCollectionOfArraysType : ConstrainedCollection<NonEmptyGuid[][]>, IConstrainedCollection<Guid[], NonGenericConstrainedCollectionOfArraysType>
	{
		NonGenericConstrainedCollectionOfArraysType(NonEmptyGuid[][] value) : base(value)
		{ }

		public static ExampleValues<IEnumerable<Guid[]>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues: ValidExampleValues.ForDuplicatesFriendlyCollectionTypes<NonEmptyGuid[]>()
				.Select(x => x.Select(y => y.Select(z => (Guid)z).ToArray())),
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid[]>>([ [ Guid.Empty ] ], "Value must not be empty") ]);

		public static NonGenericConstrainedCollectionOfArraysType ApplyConstraintsTo(IEnumerable<Guid[]> collection)
		{
			try
			{
				return new(collection.Select(guids => guids.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray()).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonGenericConstrainedCollectionOfArraysType>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonGenericConstrainedCollectionOfArraysType>(developerMistake, collection);
			}
		}
	}
}
