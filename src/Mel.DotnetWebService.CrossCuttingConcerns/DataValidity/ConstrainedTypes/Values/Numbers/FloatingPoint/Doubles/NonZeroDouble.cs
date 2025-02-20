using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroDouble : ConstrainedDouble, IConstrainedDouble<NonZeroDouble>
{
	public static ExampleValues<double> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForDoubleType.Where(d => d != default),
		constraintViolationExamples: [ ConstraintViolationExample.Document(0d, "Value must not be zero") ]);

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
