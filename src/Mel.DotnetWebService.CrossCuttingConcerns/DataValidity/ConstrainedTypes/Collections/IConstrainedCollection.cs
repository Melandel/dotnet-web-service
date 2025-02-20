using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedCollection<TElement, TSelf>
    : IConstrainedType
    where TSelf : ConstrainedType
{
	static abstract TSelf ApplyConstraintsTo(IEnumerable<TElement> collection);
	static abstract ExampleValues<IEnumerable<TElement>> Examples { get; }
}
