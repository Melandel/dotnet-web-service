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
		ICollection,
		IEnumerable,
		IList,
		IStructuralComparable,
		IStructuralEquatable,
		ICloneable,
		IList<TElement>,
		IReadOnlyList<TElement>
{
	protected ConstrainedArray(TElement[] collection) : base(collection)
	{
		var v = Array.Empty<object>();
	}

	// 👇 System.Array API
	public long LongLength => Collection.LongLength;
	public int Length => Collection.Length;
	public int Rank => Collection.Rank;

	// 👇 Interfaces API
	object? IList.this[int index] { get => ((IList)Collection)[index]; set => ((IList)Collection)[index] = value; }
	public TElement this[int index] { get => ((IList<TElement>)Collection)[index]; set => ((IList<TElement>)Collection)[index] = value; }
	TElement IReadOnlyList<TElement>.this[int index] => ((IReadOnlyList<TElement>)Collection)[index];
	public TElement[] this[Range range] => Collection[range];
	public int Count => ((ICollection)Collection).Count;
	public bool IsSynchronized => Collection.IsSynchronized;
	public object SyncRoot => Collection.SyncRoot;
	public bool IsFixedSize => Collection.IsFixedSize;
	public bool IsReadOnly => Collection.IsReadOnly;
	public int Add(object? value) => ((IList)Collection).Add(value);
	public void Add(TElement item) => ((ICollection<TElement>)Collection).Add(item);
	public void Clear() => ((IList)Collection).Clear();
	public object Clone() => Collection.Clone();
	public int CompareTo(object? other, IComparer comparer) => ((IStructuralComparable)Collection).CompareTo(other, comparer);
	public bool Contains(object? value) => ((IList)Collection).Contains(value);
	public bool Contains(TElement item) => ((ICollection<TElement>)Collection).Contains(item);
	public void CopyTo(Array array, int index) => Collection.CopyTo(array, index);
	public void CopyTo(TElement[] array, int arrayIndex) => ((ICollection<TElement>)Collection).CopyTo(array, arrayIndex);
	public bool Equals(object? other, IEqualityComparer comparer) => ((IStructuralEquatable)Collection).Equals(other, comparer);
	IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
	public int GetHashCode(IEqualityComparer comparer) => ((IStructuralEquatable)Collection).GetHashCode(comparer);
	public int IndexOf(object? value) => ((IList)Collection).IndexOf(value);
	public int IndexOf(TElement item) => ((IList<TElement>)Collection).IndexOf(item);
	public void Insert(int index, object? value) => ((IList)Collection).Insert(index, value);
	public void Insert(int index, TElement item) => ((IList<TElement>)Collection).Insert(index, item);
	public void Remove(object? value) => ((IList)Collection).Remove(value);
	public bool Remove(TElement item) => ((ICollection<TElement>)Collection).Remove(item);
	public void RemoveAt(int index) => ((IList)Collection).RemoveAt(index);
	public new IEnumerator<TElement> GetEnumerator() => ((IEnumerable<TElement>)Collection).GetEnumerator();
}
