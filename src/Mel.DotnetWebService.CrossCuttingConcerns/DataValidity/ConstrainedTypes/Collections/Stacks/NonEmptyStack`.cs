using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyStack<T> : ConstrainedList<T>, IConstrainedCollection<T, NonEmptyStack<T>>
{
	NonEmptyStack(List<T> list) : base(list)
	{
		if (Collection.Count == 0)
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
		],
		constraintViolationExamples: new[]
		{
			ConstraintViolationExample.Document<IEnumerable<T>>(
				[],
				"Collection must not be empty"),
		});

	public static NonEmptyStack<T> ApplyConstraintsTo(IEnumerable<T> collection)
	{
		try { return new(collection.ToList()); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyStack<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyStack<T>>(defect, collection); }
	}
}
