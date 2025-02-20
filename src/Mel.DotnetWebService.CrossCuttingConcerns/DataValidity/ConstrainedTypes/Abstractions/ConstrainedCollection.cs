using System.Collections;
using System.Diagnostics;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

[DebuggerDisplay($"{{{nameof(Collection)}}}")]
public class ConstrainedCollection<TCollection>
	: ConstrainedType
	where TCollection : IEnumerable
{
	protected TCollection Collection;
	protected ConstrainedCollection(TCollection collection)
	{
		Collection = collection;
	}

	public static implicit operator TCollection(ConstrainedCollection<TCollection> constrainedCollection) => constrainedCollection.Collection;
	public override string? ToString() => Collection.GetStringRepresentation();
}

public abstract class ConstrainedArray<TElement> : ConstrainedCollection<TElement[]>
{
	protected ConstrainedArray(TElement[] collection) : base(collection)
	{
	}
}

public abstract class ConstrainedList<TElement> : ConstrainedCollection<List<TElement>>
{
	protected ConstrainedList(List<TElement> collection) : base(collection)
	{
	}
}

public abstract class ConstrainedHashSet<TElement> : ConstrainedCollection<HashSet<TElement>>
{
	protected ConstrainedHashSet(HashSet<TElement> collection) : base(collection)
	{
	}
}

public abstract class ConstrainedLinkedList<TElement> : ConstrainedCollection<LinkedList<TElement>>
{
	protected ConstrainedLinkedList(LinkedList<TElement> collection) : base(collection)
	{
	}
}

public abstract class ConstrainedSortedSet<TElement> : ConstrainedCollection<SortedSet<TElement>>
{
	protected ConstrainedSortedSet(SortedSet<TElement> collection) : base(collection)
	{
	}
}

public abstract class ConstrainedStack<TElement> : ConstrainedCollection<Stack<TElement>>
{
	protected ConstrainedStack(Stack<TElement> collection) : base(collection)
	{
	}
}

public abstract class ConstrainedQueue<TElement> : ConstrainedCollection<Queue<TElement>>
{
	protected ConstrainedQueue(Queue<TElement> collection) : base(collection)
	{
	}
}
