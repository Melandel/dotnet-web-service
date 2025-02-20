namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptySortedList
{
	public static NonEmptySortedList<TKey, TValue> CreateFromSingleKeyValuePair<TKey, TValue>(TKey key, TValue value) where TKey : notnull => NonEmptySortedList<TKey, TValue>.ApplyConstraintsTo(new Dictionary<TKey, TValue>{ { key, value } });
	public static NonEmptySortedList<TKey, TValue> CreateFromSingleKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> kvp) where TKey : notnull => NonEmptySortedList<TKey, TValue>.ApplyConstraintsTo(new Dictionary<TKey, TValue> { { kvp.Key, kvp.Value } });
	public static NonEmptySortedList<TKey, TValue> CreateFromKeyValuePairs<TKey, TValue>(params KeyValuePair<TKey, TValue>[] kvps) where TKey : notnull => NonEmptySortedList<TKey, TValue>.ApplyConstraintsTo(kvps);
	public static NonEmptySortedList<TKey, TValue> ApplyConstraintsTo<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> kvps) where TKey : notnull => NonEmptySortedList<TKey, TValue>.ApplyConstraintsTo(kvps);
	public static NonEmptySortedList<TKey, TValue> ToNonEmptySortedList<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) where TKey : notnull => NonEmptySortedList<TKey, TValue>.ApplyConstraintsTo(source);
}
