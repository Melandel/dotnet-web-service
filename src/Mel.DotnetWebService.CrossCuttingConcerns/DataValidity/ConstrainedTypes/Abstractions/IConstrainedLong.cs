namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedLong<TSelf> : IConstrainedValue<long, TSelf> where TSelf : ConstrainedType
{
}
