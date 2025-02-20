using System.Collections;
using System.Diagnostics;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

[DebuggerDisplay($"{{{nameof(CollectionOfKeyValuePairs)}}}")]
public class ConstrainedCollectionOfKeyValuePairs<TKeyValuePairs>
	: ConstrainedType
	where TKeyValuePairs : IEnumerable
{
	protected TKeyValuePairs CollectionOfKeyValuePairs;
	protected ConstrainedCollectionOfKeyValuePairs(TKeyValuePairs collectionOfKeyValuePairs)
	{
		CollectionOfKeyValuePairs = collectionOfKeyValuePairs;
	}

	public static implicit operator TKeyValuePairs(ConstrainedCollectionOfKeyValuePairs<TKeyValuePairs> constrainedKeyValuePairs) => constrainedKeyValuePairs.CollectionOfKeyValuePairs;
	public override string? ToString() => CollectionOfKeyValuePairs.GetStringRepresentation();
}
