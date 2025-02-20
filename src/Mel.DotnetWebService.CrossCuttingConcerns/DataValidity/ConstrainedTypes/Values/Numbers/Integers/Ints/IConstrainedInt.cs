namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedInt<TSelf> : IConstrainedValue<int, TSelf> where TSelf : ConstrainedType
{
}
