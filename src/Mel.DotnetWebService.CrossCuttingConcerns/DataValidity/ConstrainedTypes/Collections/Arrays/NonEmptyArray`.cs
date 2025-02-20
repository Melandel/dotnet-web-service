using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyArray<T> : ConstrainedArray<T>, IConstrainedCollection<T, NonEmptyArray<T>>
{
	NonEmptyArray(T[] array) : base(array)
	{
		if (Collection.Length == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(Collection), Collection, "@member must not be empty");
		}
	}

	public static ExampleValues<IEnumerable<T>> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues:
		[
			[ Some.ExampleOf<T>() ],
			[ Some.ExampleOf<T>(), Another.ExampleOf<T>() ],
			[ Some.ExampleOf<T>(), Some.ExampleOf<T>() ],
		],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<IEnumerable<T>>( [ ], "Collection must not be empty"),
		]);

	public static NonEmptyArray<T> ApplyConstraintsTo(IEnumerable<T> collection)
	{
		try { return new(collection.ToArray()); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyArray<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyArray<T>>(defect, collection); }
	}
}
