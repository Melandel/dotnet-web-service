namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptyList
{
	public static NonEmptyList<T> CreateFromSingleElement<T>(T element) => NonEmptyList<T>.ApplyConstraintsTo(new T[] { element });
	public static NonEmptyList<T> CreateFromElements<T>(params T[] elements) => NonEmptyList<T>.ApplyConstraintsTo(elements);
	public static NonEmptyList<T> ApplyConstraintsTo<T>(IEnumerable<T> elements) => NonEmptyList<T>.ApplyConstraintsTo(elements);
	public static NonEmptyList<T> ToNonEmptyList<T>(this IEnumerable<T> source) => NonEmptyList<T>.ApplyConstraintsTo(source);
}
