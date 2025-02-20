namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedByte<TSelf> : IConstrainedValue<byte, TSelf> where TSelf : ConstrainedType
{
}
