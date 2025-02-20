namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptyQueue
{
	public static NonEmptyQueue<T> CreateFromSingleElement<T>(T element) => NonEmptyQueue<T>.ApplyConstraintsTo(new T[] { element });
	public static NonEmptyQueue<T> CreateFromElements<T>(params T[] elements) => NonEmptyQueue<T>.ApplyConstraintsTo(elements);
	public static NonEmptyQueue<T> ApplyConstraintsTo<T>(IEnumerable<T> elements) => NonEmptyQueue<T>.ApplyConstraintsTo(elements);
	public static NonEmptyQueue<T> ToNonEmptyQueue<T>(this IEnumerable<T> source) => NonEmptyQueue<T>.ApplyConstraintsTo(source);
}
