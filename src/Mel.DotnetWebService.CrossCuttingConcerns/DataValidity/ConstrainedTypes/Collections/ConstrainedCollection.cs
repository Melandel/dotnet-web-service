using System.Collections;
using System.Diagnostics;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

[DebuggerDisplay($"{{{nameof(Collection)}}}")]
public class ConstrainedCollection<TCollection>
	: ConstrainedType,
	IEnumerable
	where TCollection : IEnumerable
{
	protected TCollection Collection;
	protected ConstrainedCollection(TCollection collection)
	{
		Collection = collection;
	}

	public static implicit operator TCollection(ConstrainedCollection<TCollection> constrainedCollection) => constrainedCollection.Collection;
	public override string? ToString() => Collection.GetStringRepresentation();

	public IEnumerator GetEnumerator()
	=> Collection.GetEnumerator();
}
