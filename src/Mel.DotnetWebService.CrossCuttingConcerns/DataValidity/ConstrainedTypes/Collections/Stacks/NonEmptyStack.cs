namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptyStack
{
	public static NonEmptyStack<T> CreateFromSingleElement<T>(T element) => NonEmptyStack<T>.ApplyConstraintsTo(new T[] { element });
	public static NonEmptyStack<T> CreateFromElements<T>(params T[] elements) => NonEmptyStack<T>.ApplyConstraintsTo(elements);
	public static NonEmptyStack<T> ApplyConstraintsTo<T>(IEnumerable<T> elements) => NonEmptyStack<T>.ApplyConstraintsTo(elements);
	public static NonEmptyStack<T> ToNonEmptyStack<T>(this IEnumerable<T> source) => NonEmptyStack<T>.ApplyConstraintsTo(source);
}
