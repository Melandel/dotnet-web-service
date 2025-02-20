namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedString<TSelf> : IConstrainedValue<string, TSelf> where TSelf : ConstrainedType
{
}
