namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public interface IConstrainedDecimal<TSelf> : IConstrainedValue<decimal, TSelf> where TSelf : ConstrainedType
{
}
