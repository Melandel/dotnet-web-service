namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedFloat<TSelf> : IConstrainedValue<float, TSelf> where TSelf : ConstrainedType
{
}
