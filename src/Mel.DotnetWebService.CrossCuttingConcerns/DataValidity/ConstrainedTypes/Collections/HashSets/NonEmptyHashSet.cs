namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptyHashSet
{
	public static NonEmptyHashSet<T> CreateFromSingleElement<T>(T element) => NonEmptyHashSet<T>.ApplyConstraintsTo(new HashSet<T> { element });
	public static NonEmptyHashSet<T> CreateFromElements<T>(params T[] elements) => NonEmptyHashSet<T>.ApplyConstraintsTo(elements);
	public static NonEmptyHashSet<T> ApplyConstraintsTo<T>(IEnumerable<T> elements) => NonEmptyHashSet<T>.ApplyConstraintsTo(elements);
	public static NonEmptyHashSet<T> ToNonEmptyHashSet<T>(this IEnumerable<T> source) => NonEmptyHashSet<T>.ApplyConstraintsTo(source);
}
