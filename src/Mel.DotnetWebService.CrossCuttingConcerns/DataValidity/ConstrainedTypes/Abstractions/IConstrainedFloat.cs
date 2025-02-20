namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedFloat<TSelf> : IConstrainedValue<float, TSelf> where TSelf : ConstrainedType
{
}
