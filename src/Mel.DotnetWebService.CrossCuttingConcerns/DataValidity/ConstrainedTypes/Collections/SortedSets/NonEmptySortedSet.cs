namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptySortedSet
{
	public static NonEmptySortedSet<T> CreateFromSingleElement<T>(T element) => NonEmptySortedSet<T>.ApplyConstraintsTo(new T[] { element });
	public static NonEmptySortedSet<T> CreateFromElements<T>(params T[] elements) => NonEmptySortedSet<T>.ApplyConstraintsTo(elements);
	public static NonEmptySortedSet<T> ApplyConstraintsTo<T>(IEnumerable<T> elements) => NonEmptySortedSet<T>.ApplyConstraintsTo(elements);
	public static NonEmptySortedSet<T> ToNonEmptySortedSet<T>(this IEnumerable<T> source) => NonEmptySortedSet<T>.ApplyConstraintsTo(source);
}
