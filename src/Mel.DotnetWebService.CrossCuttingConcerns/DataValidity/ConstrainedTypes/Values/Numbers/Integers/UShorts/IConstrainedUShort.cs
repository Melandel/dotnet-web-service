namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedUShort<TSelf> : IConstrainedValue<ushort, TSelf> where TSelf : ConstrainedType
{
}
