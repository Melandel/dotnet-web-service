using System.Collections;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class ConstrainedQueue<TElement>
	: ConstrainedCollection<Queue<TElement>>,
		IEnumerable<TElement>,
		ICollection,
		IReadOnlyCollection<TElement>
{
	protected ConstrainedQueue(Queue<TElement> collection) : base(collection)
	{
	}

	// 👇 Interfaces API
	public int Count => ((ICollection)Collection).Count;

	public bool IsSynchronized => ((ICollection)Collection).IsSynchronized;

	public object SyncRoot => ((ICollection)Collection).SyncRoot;

	public void CopyTo(Array array, int index) => ((ICollection)Collection).CopyTo(array, index);

	public new IEnumerator<TElement> GetEnumerator() => ((IEnumerable<TElement>)Collection).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Collection).GetEnumerator();
}
