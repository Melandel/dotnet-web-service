using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Core;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedValue<TValue, TSelf>
    : IConstrainedType
    where TValue :
		notnull
	where TSelf :
		ConstrainedType
{
	public abstract static TSelf ApplyConstraintsTo(TValue value);
	public abstract static ExampleValues<TValue> Examples { get; }
}
