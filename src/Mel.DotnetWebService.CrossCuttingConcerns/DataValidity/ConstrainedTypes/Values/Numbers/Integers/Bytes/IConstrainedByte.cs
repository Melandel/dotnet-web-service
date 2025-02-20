namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedByte<TSelf> : IConstrainedValue<byte, TSelf> where TSelf : ConstrainedType
{
}
