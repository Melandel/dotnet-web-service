namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptySortedDictionary
{
	public static NonEmptySortedDictionary<TKey, TValue> CreateFromSingleKeyValuePair<TKey, TValue>(TKey key, TValue value) where TKey : notnull => NonEmptySortedDictionary<TKey, TValue>.ApplyConstraintsTo(new Dictionary<TKey, TValue>{ { key, value } });
	public static NonEmptySortedDictionary<TKey, TValue> CreateFromSingleKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> kvp) where TKey : notnull => NonEmptySortedDictionary<TKey, TValue>.ApplyConstraintsTo(new Dictionary<TKey, TValue> { { kvp.Key, kvp.Value } });
	public static NonEmptySortedDictionary<TKey, TValue> CreateFromKeyValuePairs<TKey, TValue>(params KeyValuePair<TKey, TValue>[] kvps) where TKey : notnull => NonEmptySortedDictionary<TKey, TValue>.ApplyConstraintsTo(kvps);
	public static NonEmptySortedDictionary<TKey, TValue> ApplyConstraintsTo<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> kvps) where TKey : notnull => NonEmptySortedDictionary<TKey, TValue>.ApplyConstraintsTo(kvps);
	public static NonEmptySortedDictionary<TKey, TValue> ToNonEmptySortedDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) where TKey : notnull => NonEmptySortedDictionary<TKey, TValue>.ApplyConstraintsTo(source);
}
