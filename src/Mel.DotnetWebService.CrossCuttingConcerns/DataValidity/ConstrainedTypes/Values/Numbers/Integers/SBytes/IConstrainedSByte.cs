namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedSByte<TSelf> : IConstrainedValue<sbyte, TSelf> where TSelf : ConstrainedType
{
}
