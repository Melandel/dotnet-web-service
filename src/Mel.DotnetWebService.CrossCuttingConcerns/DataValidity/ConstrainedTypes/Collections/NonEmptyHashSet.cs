using System.Collections;
using System.Runtime.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;
public class NonEmptyHashSet<T>
	: Constrained<HashSet<T>>,
		ICollection<T>,
		ISet<T>,
		IReadOnlyCollection<T>,
		IReadOnlySet<T>,
		IDeserializationCallback
{
	protected NonEmptyHashSet(HashSet<T> hashset) : base(hashset)
	{
		if (!Value.Any())
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuid>(nameof(Value), Value, "@member must not be empty");
		}
	}

	public static NonEmptyHashSet<T> Storing(T element)
	{
		try { return new(new HashSet<T> { element }); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(element); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, element); }
	}

	public static NonEmptyHashSet<T> Storing(IEnumerable<T> elements)
	{
		try { return new(elements.ToHashSet()); }
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(elements); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, elements); }
	}

	public int Count => ((ICollection<T>)Value).Count;
	public bool IsReadOnly => ((ICollection<T>)Value).IsReadOnly;
	public void Add(T item) => ((ICollection<T>)Value).Add(item);

	public void Clear() => ((ICollection<T>)Value).Clear();

	public bool Contains(T item) => ((ICollection<T>)Value).Contains(item);

	public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)Value).CopyTo(array, arrayIndex);

	public void ExceptWith(IEnumerable<T> other) => ((ISet<T>)Value).ExceptWith(other);

	public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Value).GetEnumerator();

	public void IntersectWith(IEnumerable<T> other) => ((ISet<T>)Value).IntersectWith(other);

	public bool IsProperSubsetOf(IEnumerable<T> other) => ((ISet<T>)Value).IsProperSubsetOf(other);

	public bool IsProperSupersetOf(IEnumerable<T> other) => ((ISet<T>)Value).IsProperSupersetOf(other);

	public bool IsSubsetOf(IEnumerable<T> other) => ((ISet<T>)Value).IsSubsetOf(other);

	public bool IsSupersetOf(IEnumerable<T> other) => ((ISet<T>)Value).IsSupersetOf(other);

	public void OnDeserialization(object? sender) => ((IDeserializationCallback)Value).OnDeserialization(sender);

	public bool Overlaps(IEnumerable<T> other) => ((ISet<T>)Value).Overlaps(other);

	public bool Remove(T item) => ((ICollection<T>)Value).Remove(item);

	public bool SetEquals(IEnumerable<T> other) => ((ISet<T>)Value).SetEquals(other);

	public void SymmetricExceptWith(IEnumerable<T> other) => ((ISet<T>)Value).SymmetricExceptWith(other);

	public void UnionWith(IEnumerable<T> other) => ((ISet<T>)Value).UnionWith(other);

	bool ISet<T>.Add(T item) => ((ISet<T>)Value).Add(item);

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Value).GetEnumerator();
}


public static class NonEmptyHashSet
{
	public static NonEmptyHashSet<T> Storing<T>(T element) => NonEmptyHashSet<T>.Storing(element);
	public static NonEmptyHashSet<T> Storing<T>(params T[] elements) => NonEmptyHashSet<T>.Storing(elements);
	public static NonEmptyHashSet<T> Storing<T>(IEnumerable<T> elements) => NonEmptyHashSet<T>.Storing(elements);
}

public static class LinqExtensionMethods
{
	public static IEnumerable<T> ToNonEmptyHashSet<T>(this IEnumerable<T> source) => NonEmptyHashSet.Storing<T>(source);
}
