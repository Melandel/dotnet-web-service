using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedSortedDictionary<TKey, TValue>
	: ConstrainedCollectionOfKeyValuePairs<SortedDictionary<TKey, TValue>>,
		IDictionary<TKey, TValue>,
		IDictionary,
		IReadOnlyDictionary<TKey, TValue> where TKey : notnull
{
	protected ConstrainedSortedDictionary(SortedDictionary<TKey, TValue> sortedDictionary) : base(sortedDictionary)
	{
	}

	// 👇 Interfaces API
	public TValue this[TKey key] { get => ((IDictionary<TKey, TValue>)CollectionOfKeyValuePairs)[key]; set => ((IDictionary<TKey, TValue>)CollectionOfKeyValuePairs)[key] = value; }
	public object? this[object key] { get => ((IDictionary)CollectionOfKeyValuePairs)[key]; set => ((IDictionary)CollectionOfKeyValuePairs)[key] = value; }

	public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)CollectionOfKeyValuePairs).Keys;

	public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)CollectionOfKeyValuePairs).Values;

	public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)CollectionOfKeyValuePairs).Count;

	public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)CollectionOfKeyValuePairs).IsReadOnly;

	public bool IsFixedSize => ((IDictionary)CollectionOfKeyValuePairs).IsFixedSize;

	public bool IsSynchronized => ((ICollection)CollectionOfKeyValuePairs).IsSynchronized;

	public object SyncRoot => ((ICollection)CollectionOfKeyValuePairs).SyncRoot;

	ICollection IDictionary.Keys => ((IDictionary)CollectionOfKeyValuePairs).Keys;

	IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)CollectionOfKeyValuePairs).Keys;

	ICollection IDictionary.Values => ((IDictionary)CollectionOfKeyValuePairs).Values;

	IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)CollectionOfKeyValuePairs).Values;

	public void Add(TKey key, TValue value) => ((IDictionary<TKey, TValue>)CollectionOfKeyValuePairs).Add(key, value);

	public void Add(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)CollectionOfKeyValuePairs).Add(item);

	public void Add(object key, object? value) => ((IDictionary)CollectionOfKeyValuePairs).Add(key, value);

	public void Clear() => ((ICollection<KeyValuePair<TKey, TValue>>)CollectionOfKeyValuePairs).Clear();

	public bool Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)CollectionOfKeyValuePairs).Contains(item);

	public bool Contains(object key) => ((IDictionary)CollectionOfKeyValuePairs).Contains(key);

	public bool ContainsKey(TKey key) => ((IDictionary<TKey, TValue>)CollectionOfKeyValuePairs).ContainsKey(key);

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)CollectionOfKeyValuePairs).CopyTo(array, arrayIndex);

	public void CopyTo(Array array, int index) => ((ICollection)CollectionOfKeyValuePairs).CopyTo(array, index);

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IEnumerable<KeyValuePair<TKey, TValue>>)CollectionOfKeyValuePairs).GetEnumerator();

	public bool Remove(TKey key) => ((IDictionary<TKey, TValue>)CollectionOfKeyValuePairs).Remove(key);

	public bool Remove(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)CollectionOfKeyValuePairs).Remove(item);

	public void Remove(object key) => ((IDictionary)CollectionOfKeyValuePairs).Remove(key);

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => ((IDictionary<TKey, TValue>)CollectionOfKeyValuePairs).TryGetValue(key, out value);

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)CollectionOfKeyValuePairs).GetEnumerator();

	IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary)CollectionOfKeyValuePairs).GetEnumerator();
}
