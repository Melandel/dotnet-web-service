namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedString<TSelf> : IConstrainedValue<string, TSelf> where TSelf : ConstrainedType
{
}
