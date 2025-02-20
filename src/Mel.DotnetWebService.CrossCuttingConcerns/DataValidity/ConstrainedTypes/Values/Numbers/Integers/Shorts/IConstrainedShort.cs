namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedShort<TSelf> : IConstrainedValue<short, TSelf> where TSelf : ConstrainedType
{
}
