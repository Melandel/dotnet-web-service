namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptyLinkedList
{
	public static NonEmptyLinkedList<T> CreateFromSingleElement<T>(T element) => NonEmptyLinkedList<T>.ApplyConstraintsTo(new T[] { element });
	public static NonEmptyLinkedList<T> CreateFromElements<T>(params T[] elements) => NonEmptyLinkedList<T>.ApplyConstraintsTo(elements);
	public static NonEmptyLinkedList<T> ApplyConstraintsTo<T>(IEnumerable<T> elements) => NonEmptyLinkedList<T>.ApplyConstraintsTo(elements);
	public static NonEmptyLinkedList<T> ToNonEmptyLinkedList<T>(this IEnumerable<T> source) => NonEmptyLinkedList<T>.ApplyConstraintsTo(source);
}
