using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonEmptyStack<T> : ConstrainedStack<T>, IConstrainedCollection<T, NonEmptyStack<T>>
{
	NonEmptyStack(Stack<T> stack) : base(stack)
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
		try { return new(new Stack<T>(collection)); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyStack<T>>(collection); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyStack<T>>(defect, collection); }
	}
}
