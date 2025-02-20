namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedULong<TSelf> : IConstrainedValue<ulong, TSelf> where TSelf : ConstrainedType
{
}
