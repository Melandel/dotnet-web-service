namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class ArrayOfUniqueValuesWithAtLeast2Items
{
	public static ArrayOfUniqueValuesWithAtLeast2Items<T> CreateFromTwoElements<T>(T firstElement, T secondElement) => ArrayOfUniqueValuesWithAtLeast2Items<T>.ApplyConstraintsTo(new[] { firstElement, secondElement });
	public static ArrayOfUniqueValuesWithAtLeast2Items<T> CreateFromElements<T>(params T[] collection) => ArrayOfUniqueValuesWithAtLeast2Items<T>.ApplyConstraintsTo(collection);
	public static ArrayOfUniqueValuesWithAtLeast2Items<T> ApplyConstraintsTo<T>(IEnumerable<T> collection) => ArrayOfUniqueValuesWithAtLeast2Items<T>.ApplyConstraintsTo(collection);

	public static ArrayOfUniqueValuesWithAtLeast2Items<T> ToArrayOfUniqueValuesWithAtLeast2Items<T>(this IEnumerable<T> source) => ArrayOfUniqueValuesWithAtLeast2Items<T>.ApplyConstraintsTo(source);
}
