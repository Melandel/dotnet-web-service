using System.Collections;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedList<TKey>
	: ConstrainedCollection<List<TKey>>,
		IList<TKey>,
		IList,
		IReadOnlyList<TKey>
{
	protected ConstrainedList(List<TKey> collection) : base(collection)
	{
	}

	// 👇 Interfaces API
	public TKey this[int index] { get => ((IList<TKey>)Collection)[index]; set => ((IList<TKey>)Collection)[index] = value; }
	object? IList.this[int index] { get => ((IList)Collection)[index]; set => ((IList)Collection)[index] = value; }

	public int Count => ((ICollection<TKey>)Collection).Count;

	public bool IsReadOnly => ((ICollection<TKey>)Collection).IsReadOnly;

	public bool IsFixedSize => ((IList)Collection).IsFixedSize;

	public bool IsSynchronized => ((ICollection)Collection).IsSynchronized;

	public object SyncRoot => ((ICollection)Collection).SyncRoot;

	public void Add(TKey item) => ((ICollection<TKey>)Collection).Add(item);

	public int Add(object? value) => ((IList)Collection).Add(value);

	public void Clear() => ((ICollection<TKey>)Collection).Clear();

	public bool Contains(TKey item) => ((ICollection<TKey>)Collection).Contains(item);

	public bool Contains(object? value) => ((IList)Collection).Contains(value);

	public void CopyTo(TKey[] array, int arrayIndex) => ((ICollection<TKey>)Collection).CopyTo(array, arrayIndex);

	public void CopyTo(Array array, int index) => ((ICollection)Collection).CopyTo(array, index);

	public new IEnumerator<TKey> GetEnumerator() => ((IEnumerable<TKey>)Collection).GetEnumerator();

	public int IndexOf(TKey item) => ((IList<TKey>)Collection).IndexOf(item);

	public int IndexOf(object? value) => ((IList)Collection).IndexOf(value);

	public void Insert(int index, TKey item) => ((IList<TKey>)Collection).Insert(index, item);

	public void Insert(int index, object? value) => ((IList)Collection).Insert(index, value);

	public bool Remove(TKey item) => ((ICollection<TKey>)Collection).Remove(item);

	public void Remove(object? value) => ((IList)Collection).Remove(value);

	public void RemoveAt(int index) => ((IList<TKey>)Collection).RemoveAt(index);

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Collection).GetEnumerator();
}
