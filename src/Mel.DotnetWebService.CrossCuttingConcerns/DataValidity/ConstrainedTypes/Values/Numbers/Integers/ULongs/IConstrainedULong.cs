namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedULong<TSelf> : IConstrainedValue<ulong, TSelf> where TSelf : ConstrainedType
{
}
