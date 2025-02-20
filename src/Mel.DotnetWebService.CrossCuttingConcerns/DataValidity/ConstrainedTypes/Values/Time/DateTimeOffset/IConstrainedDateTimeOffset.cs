namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedDateTimeOffset<TSelf> : IConstrainedValue<DateTimeOffset, TSelf> where TSelf : ConstrainedType
{
}
