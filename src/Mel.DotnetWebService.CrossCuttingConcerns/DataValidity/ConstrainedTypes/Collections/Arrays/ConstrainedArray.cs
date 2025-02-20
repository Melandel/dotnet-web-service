using System.Collections;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

// 👇 This class implements IList<T> and IReadOnlyList<T>
// Justification : This data structure's API should look just like T[]'s API
// See https://learn.microsoft.com/en-us/dotnet/api/system.array?view=net-10.0#remarks
//   Single-dimensional arrays implement the System.Collections.Generic.IList<T>, System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>,
//   System.Collections.Generic.IReadOnlyList<T> and System.Collections.Generic.IReadOnlyCollection<T> generic interfaces. The implementations are provided to arrays
//   at run time, and as a result, the generic interfaces do not appear in the declaration syntax for the Array class. In addition, there are no reference topics for
//   interface members that are accessible only by casting an array to the generic interface type (explicit interface implementations). The key thing to be aware of
//   when you cast an array to one of these interfaces is that members which add, insert, or remove elements throw NotSupportedException.
public abstract class ConstrainedArray<TElement>
	: ConstrainedCollection<TElement[]>,
		IList<TElement>,
		IReadOnlyList<TElement>
{
	protected ConstrainedArray(TElement[] collection) : base(collection)
	{
	}

	// 👇 System.Array API
	public object SyncRoot => Collection.SyncRoot;
	public long LongLength => Collection.LongLength;
	public int Length => Collection.Length;
	public bool IsSynchronized => Collection.IsSynchronized;
	public bool IsFixedSize => Collection.IsFixedSize;
	public int Rank => Collection.Rank;
	public object Clone() => Collection.Clone();
	public void CopyTo(Array array, long index) => Collection.CopyTo(array, index);
	public void CopyTo(Array array, int index) => Collection.CopyTo(array, index);

	// 👇 Interfaces API
	public TElement this[int index]
	{
		get => ((IList<TElement>)Collection)[index];
		set => ((IList<TElement>)Collection)[index] = value;
	}
	public int Count => ((ICollection<TElement>)Collection).Count;
	public bool IsReadOnly => ((ICollection<TElement>)Collection).IsReadOnly;
	public void Add(TElement item) => ((ICollection<TElement>)Collection).Add(item);
	public void Clear() => ((ICollection<TElement>)Collection).Clear();
	public bool Contains(TElement item) => ((ICollection<TElement>)Collection).Contains(item);
	public void CopyTo(TElement[] array, int arrayIndex) => ((ICollection<TElement>)Collection).CopyTo(array, arrayIndex);
	public IEnumerator<TElement> GetEnumerator() => ((IEnumerable<TElement>)Collection).GetEnumerator();
	public int IndexOf(TElement item) => ((IList<TElement>)Collection).IndexOf(item);
	public void Insert(int index, TElement item) => ((IList<TElement>)Collection).Insert(index, item);
	public bool Remove(TElement item) => ((ICollection<TElement>)Collection).Remove(item);
	public void RemoveAt(int index) => ((IList<TElement>)Collection).RemoveAt(index);
	IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
}
