namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstraineGuid<TSelf> : IConstrainedValue<Guid, TSelf> where TSelf : ConstrainedType
{
}
