namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedUInt<TSelf> : IConstrainedValue<uint, TSelf> where TSelf : ConstrainedType
{
}
