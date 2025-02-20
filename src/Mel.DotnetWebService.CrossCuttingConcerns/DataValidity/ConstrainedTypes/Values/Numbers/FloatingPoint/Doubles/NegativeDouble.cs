using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NegativeDouble : ConstrainedDouble, IConstrainedDouble<NegativeDouble>
{
	public static ExampleValues<double> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForDoubleType.Where(d => d <= 0d),
		constraintViolationExamples: [ ConstraintViolationExample.Document(double.Epsilon, "Value must be equal to or less than zero") ]);

	NegativeDouble(double value) : base(value)
	{
		if (Value > 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NegativeDouble>(nameof(Value), value, "@member must be equal to or less than zero");
		}
	}

	public static NegativeDouble ApplyConstraintsTo(double scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NegativeDouble>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NegativeDouble>(defect, scalarValue); }
	}
}
