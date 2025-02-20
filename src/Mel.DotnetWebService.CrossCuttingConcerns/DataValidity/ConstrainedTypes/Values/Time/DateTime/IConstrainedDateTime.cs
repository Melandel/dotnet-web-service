namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedDateTime<TSelf> : IConstrainedValue<DateTime, TSelf> where TSelf : ConstrainedType
{
}
