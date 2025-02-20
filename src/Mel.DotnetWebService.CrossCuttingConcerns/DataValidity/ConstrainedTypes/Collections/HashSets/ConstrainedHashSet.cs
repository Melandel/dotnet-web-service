using System.Collections;
using System.Runtime.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

// 👇 This class implements ICollection<T>, ISet<T>, IReadOnlyCollection<T>, IReadOnlySet<T> and IDeserializationCallback
// Justification : This data structure's API should look just like System.Collections.Generic.HashSet<T>'s API
//   See https://github.com/dotnet/runtime/blob/242f7b23752599f22157268de41fee91cb97ef6c/src/libraries/System.Private.CoreLib/src/System/Collections/Generic/HashSet.cs#L17
public abstract class ConstrainedHashSet<T>
	: ConstrainedCollection<HashSet<T>>,
		ICollection<T>,
		ISet<T>,
		IReadOnlyCollection<T>,
		IReadOnlySet<T>,
		IDeserializationCallback
{
	protected ConstrainedHashSet(HashSet<T> collection) : base(collection)
	{
	}

	public int Count => ((ICollection<T>)Collection).Count;
	public bool IsReadOnly => ((ICollection<T>)Collection).IsReadOnly;
	public void Add(T item) => ((ICollection<T>)Collection).Add(item);
	public void Clear() => ((ICollection<T>)Collection).Clear();
	public bool Contains(T item) => ((ICollection<T>)Collection).Contains(item);
	public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)Collection).CopyTo(array, arrayIndex);
	public void ExceptWith(IEnumerable<T> other) => ((ISet<T>)Collection).ExceptWith(other);
	public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Collection).GetEnumerator();
	public void IntersectWith(IEnumerable<T> other) => ((ISet<T>)Collection).IntersectWith(other);
	public bool IsProperSubsetOf(IEnumerable<T> other) => ((ISet<T>)Collection).IsProperSubsetOf(other);
	public bool IsProperSupersetOf(IEnumerable<T> other) => ((ISet<T>)Collection).IsProperSupersetOf(other);
	public bool IsSubsetOf(IEnumerable<T> other) => ((ISet<T>)Collection).IsSubsetOf(other);
	public bool IsSupersetOf(IEnumerable<T> other) => ((ISet<T>)Collection).IsSupersetOf(other);
	public void OnDeserialization(object? sender) => ((IDeserializationCallback)Collection).OnDeserialization(sender);
	public bool Overlaps(IEnumerable<T> other) => ((ISet<T>)Collection).Overlaps(other);
	public bool Remove(T item) => ((ICollection<T>)Collection).Remove(item);
	public bool SetEquals(IEnumerable<T> other) => ((ISet<T>)Collection).SetEquals(other);
	public void SymmetricExceptWith(IEnumerable<T> other) => ((ISet<T>)Collection).SymmetricExceptWith(other);
	public void UnionWith(IEnumerable<T> other) => ((ISet<T>)Collection).UnionWith(other);
	bool ISet<T>.Add(T item) => ((ISet<T>)Collection).Add(item);
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Collection).GetEnumerator();
}
