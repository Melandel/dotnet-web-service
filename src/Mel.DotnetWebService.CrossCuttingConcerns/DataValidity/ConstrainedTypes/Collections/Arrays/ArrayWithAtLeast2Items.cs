namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class ArrayWithAtLeast2Items
{
	public static ArrayWithAtLeast2Items<T> CreateFromTwoElements<T>(T firstElement, T secondElement) => ArrayWithAtLeast2Items<T>.ApplyConstraintsTo(new[] { firstElement, secondElement });
	public static ArrayWithAtLeast2Items<T> CreateFromElements<T>(params T[] collection) => ArrayWithAtLeast2Items<T>.ApplyConstraintsTo(collection);
	public static ArrayWithAtLeast2Items<T> ApplyConstraintsTo<T>(IEnumerable<T> collection) => ArrayWithAtLeast2Items<T>.ApplyConstraintsTo(collection);

	public static ArrayWithAtLeast2Items<T> ToArrayWithAtLeast2Items<T>(this IEnumerable<T> source) => ArrayWithAtLeast2Items<T>.ApplyConstraintsTo(source);
}
