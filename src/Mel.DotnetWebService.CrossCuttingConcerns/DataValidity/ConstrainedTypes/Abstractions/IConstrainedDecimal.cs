namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public interface IConstrainedDecimal<TSelf> : IConstrainedValue<decimal, TSelf> where TSelf : ConstrainedType
{
}
