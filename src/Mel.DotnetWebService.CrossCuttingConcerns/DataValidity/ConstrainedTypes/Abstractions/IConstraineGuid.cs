namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstraineGuid<TSelf> : IConstrainedValue<Guid, TSelf> where TSelf : ConstrainedType
{
}
