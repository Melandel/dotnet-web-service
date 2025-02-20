using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Core;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroDouble : ConstrainedDouble, IConstrainedDouble<NonZeroDouble>
{
	public static ExampleValues<double> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new[] { 1d, 2.5d, -1.5d, -1.7976931348623157E+308d, 1.7976931348623157E+308d },
		errorMessagesByInvalidValue: new Dictionary<double, string>
		{
			{ 0, "Value must not be zero" }
		});

	NonZeroDouble(double value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroDouble>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroDouble ApplyConstraintsTo(double scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroDouble>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroDouble>(defect, scalarValue); }
	}
}
