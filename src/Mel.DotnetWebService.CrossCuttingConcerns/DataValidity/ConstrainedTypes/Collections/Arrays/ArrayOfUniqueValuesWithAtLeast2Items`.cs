using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class ArrayOfUniqueValuesWithAtLeast2Items<T> : ConstrainedArray<T>, IConstrainedCollection<T, ArrayOfUniqueValuesWithAtLeast2Items<T>>
{
	ArrayOfUniqueValuesWithAtLeast2Items(T[] value) : base(value)
	{
		if (Collection.Length < 2)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<ArrayOfUniqueValuesWithAtLeast2Items<T>>(nameof(value), value, $"@member must have at least 2 elements, but instead has {Collection.Length} element");
		}

		if (Collection.Distinct().Count() != Collection.Length)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<ArrayOfUniqueValuesWithAtLeast2Items<T>>(nameof(value), value, $"@member must have no duplicate elements");
		}
	}

	public static ExampleValues<IEnumerable<T>> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues:
		[
			[ Some.ExampleOf<T>(), Another.ExampleOf<T>() ],
			[ Another.ExampleOf<T>(), YetAnother.ExampleOf<T>() ],
		],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<IEnumerable<T>>( [ ],                     "Collection must have at least 2 elements, but instead has 0 element"),
			ConstraintViolationExample.Document<IEnumerable<T>>( [ Some.ExampleOf<T>() ], "Collection must have at least 2 elements, but instead has 1 element"),
			ConstraintViolationExample.Document<IEnumerable<T>>( [ Some.ExampleOf<T>(), Some.ExampleOf<T>() ], "Collection must have no duplicate elements"),
		]);


	public static ArrayOfUniqueValuesWithAtLeast2Items<T> ApplyConstraintsTo(IEnumerable<T> collection)
	{
		try { return new(collection.ToArray()); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, collection); }
	}
}
