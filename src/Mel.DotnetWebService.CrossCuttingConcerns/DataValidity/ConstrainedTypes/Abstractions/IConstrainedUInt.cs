namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedUInt<TSelf> : IConstrainedValue<uint, TSelf> where TSelf : ConstrainedType
{
}
