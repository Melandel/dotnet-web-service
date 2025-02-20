using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class PositiveShort : ConstrainedShort, IConstrainedShort<PositiveShort>
{
	public static ExampleValues<short> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: ExampleValues.ForShortType.Where(s => s >= 0),
		constraintViolationExamples: [ ConstraintViolationExample.Document<short>(-1, "Value must be equal to or greater than zero") ]);

	PositiveShort(short value) : base(value)
	{
		if (Value < 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<PositiveShort>(nameof(Value), value, "@member must be equal to or greater than zero");
		}
	}

	public static PositiveShort ApplyConstraintsTo(short scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<PositiveShort>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<PositiveShort>(defect, scalarValue); }
	}
}
