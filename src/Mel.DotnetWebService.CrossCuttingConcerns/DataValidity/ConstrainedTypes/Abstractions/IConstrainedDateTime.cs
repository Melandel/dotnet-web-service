namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedDateTime<TSelf> : IConstrainedValue<DateTime, TSelf> where TSelf : ConstrainedType
{
}
