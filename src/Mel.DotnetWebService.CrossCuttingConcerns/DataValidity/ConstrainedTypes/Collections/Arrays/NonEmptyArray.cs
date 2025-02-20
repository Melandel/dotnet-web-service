namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptyArray
{
	public static NonEmptyArray<T> CreateFromSingleElement<T>(T element) => NonEmptyArray<T>.ApplyConstraintsTo(new T[] { element });
	public static NonEmptyArray<T> CreateFromElements<T>(params T[] elements) => NonEmptyArray<T>.ApplyConstraintsTo(elements);
	public static NonEmptyArray<T> ApplyConstraintsTo<T>(IEnumerable<T> elements) => NonEmptyArray<T>.ApplyConstraintsTo(elements);
	public static NonEmptyArray<T> ToNonEmptyArray<T>(this IEnumerable<T> source) => NonEmptyArray<T>.ApplyConstraintsTo(source);
}
