using System.Collections;
using System.Runtime.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedSortedSet<TElement>
	: ConstrainedCollection<SortedSet<TElement>>,
		ISet<TElement>,
		ICollection<TElement>,
		ICollection,
		IReadOnlyCollection<TElement>,
		IReadOnlySet<TElement>,
		ISerializable,
		IDeserializationCallback
{
	protected ConstrainedSortedSet(SortedSet<TElement> collection) : base(collection)
	{
	}

	// 👇 Interfaces API
	public int Count => ((ICollection<TElement>)Collection).Count;

	public bool IsReadOnly => ((ICollection<TElement>)Collection).IsReadOnly;

	public bool IsSynchronized => ((ICollection)Collection).IsSynchronized;

	public object SyncRoot => ((ICollection)Collection).SyncRoot;

	public bool Add(TElement item) => ((ISet<TElement>)Collection).Add(item);

	public void Clear() => ((ICollection<TElement>)Collection).Clear();

	public bool Contains(TElement item) => ((ICollection<TElement>)Collection).Contains(item);

	public void CopyTo(TElement[] array, int arrayIndex) => ((ICollection<TElement>)Collection).CopyTo(array, arrayIndex);

	public void CopyTo(Array array, int index) => ((ICollection)Collection).CopyTo(array, index);

	public void ExceptWith(IEnumerable<TElement> other) => ((ISet<TElement>)Collection).ExceptWith(other);

	public new IEnumerator<TElement> GetEnumerator() => ((IEnumerable<TElement>)Collection).GetEnumerator();

#pragma warning disable
	public void GetObjectData(SerializationInfo info, StreamingContext context) => ((ISerializable)Collection).GetObjectData(info, context);
#pragma warning restore
	public void IntersectWith(IEnumerable<TElement> other) => ((ISet<TElement>)Collection).IntersectWith(other);

	public bool IsProperSubsetOf(IEnumerable<TElement> other) => ((ISet<TElement>)Collection).IsProperSubsetOf(other);

	public bool IsProperSupersetOf(IEnumerable<TElement> other) => ((ISet<TElement>)Collection).IsProperSupersetOf(other);

	public bool IsSubsetOf(IEnumerable<TElement> other) => ((ISet<TElement>)Collection).IsSubsetOf(other);

	public bool IsSupersetOf(IEnumerable<TElement> other) => ((ISet<TElement>)Collection).IsSupersetOf(other);

	public void OnDeserialization(object? sender) => ((IDeserializationCallback)Collection).OnDeserialization(sender);

	public bool Overlaps(IEnumerable<TElement> other) => ((ISet<TElement>)Collection).Overlaps(other);

	public bool Remove(TElement item) => ((ICollection<TElement>)Collection).Remove(item);

	public bool SetEquals(IEnumerable<TElement> other)
	=> ((ISet<TElement>)Collection).SetEquals(other);

	public void SymmetricExceptWith(IEnumerable<TElement> other)
	=> ((ISet<TElement>)Collection).SymmetricExceptWith(other);

	public void UnionWith(IEnumerable<TElement> other)
	=> ((ISet<TElement>)Collection).UnionWith(other);

	void ICollection<TElement>.Add(TElement item) => ((ICollection<TElement>)Collection).Add(item);

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Collection).GetEnumerator();
}
