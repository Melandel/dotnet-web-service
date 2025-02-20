using System.Collections;
using System.Runtime.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedLinkedList<TElement>
	: ConstrainedCollection<LinkedList<TElement>>,
		ICollection<TElement>,
		ICollection,
		IReadOnlyCollection<TElement>,
		ISerializable,
		IDeserializationCallback
{
	protected ConstrainedLinkedList(LinkedList<TElement> collection) : base(collection)
	{
	}

	// 👇 Interfaces API
	public int Count => ((ICollection<TElement>)Collection).Count;

	public bool IsReadOnly => ((ICollection<TElement>)Collection).IsReadOnly;

	public bool IsSynchronized => ((ICollection)Collection).IsSynchronized;

	public object SyncRoot => ((ICollection)Collection).SyncRoot;

	public void Add(TElement item) => ((ICollection<TElement>)Collection).Add(item);

	public void Clear() => ((ICollection<TElement>)Collection).Clear();

	public bool Contains(TElement item) => ((ICollection<TElement>)Collection).Contains(item);

	public void CopyTo(TElement[] array, int arrayIndex) => ((ICollection<TElement>)Collection).CopyTo(array, arrayIndex);

	public void CopyTo(Array array, int index) => ((ICollection)Collection).CopyTo(array, index);

	public new IEnumerator<TElement> GetEnumerator() => ((IEnumerable<TElement>)Collection).GetEnumerator();

#pragma warning disable
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	=> ((ISerializable)Collection).GetObjectData(info, context);
#pragma warning restore
	public void OnDeserialization(object? sender)
	=> ((IDeserializationCallback)Collection).OnDeserialization(sender);

	public bool Remove(TElement item)
	=> ((ICollection<TElement>)Collection).Remove(item);
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Collection).GetEnumerator();
}
