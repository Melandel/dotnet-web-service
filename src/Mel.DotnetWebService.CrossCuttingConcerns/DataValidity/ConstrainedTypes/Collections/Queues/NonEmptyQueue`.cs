using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyQueue<T> : ConstrainedQueue<T>, IConstrainedCollection<T, NonEmptyQueue<T>>
{
	NonEmptyQueue(Queue<T> queue) : base(queue)
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

	public static NonEmptyQueue<T> ApplyConstraintsTo(IEnumerable<T> collection)
	{
		try { return new(new Queue<T>(collection)); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyQueue<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyQueue<T>>(defect, collection); }
	}
}
