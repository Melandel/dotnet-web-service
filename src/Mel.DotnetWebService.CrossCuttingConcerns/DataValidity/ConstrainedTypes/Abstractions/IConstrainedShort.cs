namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedShort<TSelf> : IConstrainedValue<short, TSelf> where TSelf : ConstrainedType
{
}
