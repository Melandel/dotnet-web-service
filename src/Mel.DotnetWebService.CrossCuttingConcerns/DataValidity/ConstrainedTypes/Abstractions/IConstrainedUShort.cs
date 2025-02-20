namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedUShort<TSelf> : IConstrainedValue<ushort, TSelf> where TSelf : ConstrainedType
{
}
