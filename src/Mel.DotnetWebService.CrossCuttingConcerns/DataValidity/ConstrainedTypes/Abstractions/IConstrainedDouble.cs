namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedDouble<TSelf> : IConstrainedValue<double, TSelf> where TSelf : ConstrainedType
{
}
