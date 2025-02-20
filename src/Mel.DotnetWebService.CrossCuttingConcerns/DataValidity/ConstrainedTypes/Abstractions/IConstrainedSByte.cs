namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedSByte<TSelf> : IConstrainedValue<sbyte, TSelf> where TSelf : ConstrainedType
{
}
