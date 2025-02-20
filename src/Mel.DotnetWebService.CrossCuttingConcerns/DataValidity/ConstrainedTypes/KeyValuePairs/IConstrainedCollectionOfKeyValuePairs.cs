using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedCollectionOfKeyValuePairs<TKey, TValue, TSelf>
    : IConstrainedType
    where TSelf : ConstrainedType
{
	static abstract TSelf ApplyConstraintsTo(IEnumerable<KeyValuePair<TKey, TValue>> collectionOfKeyValuePairs);
	static abstract ExampleValues<IEnumerable<KeyValuePair<TKey, TValue>>> Examples { get; }
}
