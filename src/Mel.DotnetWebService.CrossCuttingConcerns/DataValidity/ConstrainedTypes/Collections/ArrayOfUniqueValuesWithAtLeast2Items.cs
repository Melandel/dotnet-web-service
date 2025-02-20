using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

// 👇 This class implements IList<T> and IReadOnlyList<T>
// Justification : This data structure's API should look just like T[]'s API
// See https://learn.microsoft.com/en-us/dotnet/api/system.array?view=net-10.0#remarks
//   Single-dimensional arrays implement the System.Collections.Generic.IList<T>, System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>,
//   System.Collections.Generic.IReadOnlyList<T> and System.Collections.Generic.IReadOnlyCollection<T> generic interfaces. The implementations are provided to arrays
//   at run time, and as a result, the generic interfaces do not appear in the declaration syntax for the Array class. In addition, there are no reference topics for
//   interface members that are accessible only by casting an array to the generic interface type (explicit interface implementations). The key thing to be aware of
//   when you cast an array to one of these interfaces is that members which add, insert, or remove elements throw NotSupportedException.
public sealed class ArrayOfUniqueValuesWithAtLeast2Items<T> : ConstrainedCollection<T[]>, IList<T>, IReadOnlyList<T>
{
	ArrayOfUniqueValuesWithAtLeast2Items(T[] value) : base(value)
	{
		Array.Empty<object>();
		if (Collection.Length < 2)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<ArrayOfUniqueValuesWithAtLeast2Items<T>>(nameof(value), value, $"@member must have at least 2 elements, but instead has {Collection.Length} elements");
		}

		if (Collection.Distinct().Count() != Collection.Length)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<ArrayOfUniqueValuesWithAtLeast2Items<T>>(nameof(value), value, $"@member must have no duplicate elements");
		}
	}

	public static ArrayOfUniqueValuesWithAtLeast2Items<T> Storing(T firstElement, T secondElement)
	{
		try { return new(new[] { firstElement, secondElement }); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(firstElement, secondElement); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, firstElement, secondElement); }

	}

	public static ArrayOfUniqueValuesWithAtLeast2Items<T> Storing(IEnumerable<T> elements)
	{
		try { return new(elements.ToArray()); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(elements); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, elements); }
	}

	// 👇 System.Array API
	public object SyncRoot => Collection.SyncRoot;
	public long LongLength => Collection.LongLength;
	public int Length => Collection.Length;
	public bool IsSynchronized => Collection.IsSynchronized;
	public bool IsFixedSize => Collection.IsFixedSize;
	public int Rank => Collection.Rank;
	public object Clone() => Collection.Clone();
	public void CopyTo(Array array, long index) { Collection.CopyTo(array, index); }
	public void CopyTo(Array array, int index)  { Collection.CopyTo(array, index); }

	// 👇 Interfaces APIs
	public T this[int index] { get => ((IList<T>)Collection)[index]; set => ((IList<T>)Collection)[index] = value; }
	public int Count => ((ICollection<T>)Collection).Count;
	public bool IsReadOnly => ((ICollection<T>)Collection).IsReadOnly;
	public void Add(T item) => ((ICollection<T>)Collection).Add(item);
	public void Clear() => ((ICollection<T>)Collection).Clear();
	public bool Contains(T item) => ((ICollection<T>)Collection).Contains(item);
	public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)Collection).CopyTo(array, arrayIndex);
	public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Collection).GetEnumerator();
	public int IndexOf(T item) => ((IList<T>)Collection).IndexOf(item);
	public void Insert(int index, T item) => ((IList<T>)Collection).Insert(index, item);
	public bool Remove(T item) => ((ICollection<T>)Collection).Remove(item);
	public void RemoveAt(int index) => ((IList<T>)Collection).RemoveAt(index);
	IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
}

public static class ArrayOfUniqueValuesWithAtLeast2Items
{
	public static ArrayOfUniqueValuesWithAtLeast2Items<T> Storing<T>(T firstElement, T secondElement) => ArrayOfUniqueValuesWithAtLeast2Items<T>.Storing(firstElement, secondElement);
	public static ArrayOfUniqueValuesWithAtLeast2Items<T> Storing<T>(params T[] elements) => ArrayOfUniqueValuesWithAtLeast2Items<T>.Storing(elements);
	public static ArrayOfUniqueValuesWithAtLeast2Items<T> Storing<T>(IEnumerable<T> elements)
	=> ArrayOfUniqueValuesWithAtLeast2Items<T>.Storing(elements);
}
