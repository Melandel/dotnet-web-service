namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedLong<TSelf> : IConstrainedValue<long, TSelf> where TSelf : ConstrainedType
{
}
