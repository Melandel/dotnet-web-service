namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedCollection<TElement, TSelf>
    : IConstrainedType
    where TSelf : ConstrainedType
{
	public abstract static TSelf ApplyConstraintsTo(IEnumerable<TElement> collection);
}
