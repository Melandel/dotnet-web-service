namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedInt<TSelf> : IConstrainedValue<int, TSelf> where TSelf : ConstrainedType
{
}
