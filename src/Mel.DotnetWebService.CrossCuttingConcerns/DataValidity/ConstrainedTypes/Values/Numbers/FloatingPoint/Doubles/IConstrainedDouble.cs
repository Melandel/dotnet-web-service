namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedDouble<TSelf> : IConstrainedValue<double, TSelf> where TSelf : ConstrainedType
{
}
