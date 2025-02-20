namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public static class NonEmptyDictionary
{
	public static NonEmptyDictionary<TKey, TValue> CreateFromSingleKeyValuePair<TKey, TValue>(TKey key, TValue value) where TKey : notnull => NonEmptyDictionary<TKey, TValue>.ApplyConstraintsTo(new Dictionary<TKey, TValue>{ { key, value } });
	public static NonEmptyDictionary<TKey, TValue> CreateFromSingleKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> kvp) where TKey : notnull => NonEmptyDictionary<TKey, TValue>.ApplyConstraintsTo(new Dictionary<TKey, TValue> { { kvp.Key, kvp.Value } });
	public static NonEmptyDictionary<TKey, TValue> CreateFromKeyValuePairs<TKey, TValue>(params KeyValuePair<TKey, TValue>[] kvps) where TKey : notnull => NonEmptyDictionary<TKey, TValue>.ApplyConstraintsTo(kvps);
	public static NonEmptyDictionary<TKey, TValue> ApplyConstraintsTo<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> kvps) where TKey : notnull => NonEmptyDictionary<TKey, TValue>.ApplyConstraintsTo(kvps);
	public static NonEmptyDictionary<TKey, TValue> ToNonEmptyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) where TKey : notnull => NonEmptyDictionary<TKey, TValue>.ApplyConstraintsTo(source);
}
