using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class ArrayWithAtLeast2Items<T> : ConstrainedArray<T>, IConstrainedCollection<T, ArrayWithAtLeast2Items<T>>
{
	ArrayWithAtLeast2Items(T[] value) : base(value)
	{
		if (Collection.Length < 2)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<ArrayWithAtLeast2Items<T>>(nameof(value), value, $"@member must have at least 2 elements, but instead has {Collection.Length} element");
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
		]);


	public static ArrayWithAtLeast2Items<T> ApplyConstraintsTo(IEnumerable<T> collection)
	{
		try { return new(collection.ToArray()); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, collection); }
	}
}
