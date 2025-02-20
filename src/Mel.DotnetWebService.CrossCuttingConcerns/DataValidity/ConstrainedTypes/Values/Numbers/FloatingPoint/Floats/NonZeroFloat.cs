namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public sealed class NonZeroFloat : ConstrainedFloat, IConstrainedFloat<NonZeroFloat>
{
	public static ExampleValues<float> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new[] { 1f, 2.5f, -1.5f, 3.40282346638528859e+38f, -3.40282346638528859e+38f },
		errorMessagesByInvalidValue: new Dictionary<float, string>
		{
			{ 0, "Value must not be zero" }
		});

	NonZeroFloat(float value) : base(value)
	{
		if (Value == 0)
		{
			throw ObjectConstructionException.WhenConstructingAMemberFor<NonZeroFloat>(nameof(Value), value, "@member must not be zero");
		}
	}

	public static NonZeroFloat ApplyConstraintsTo(float scalarValue)
	{
		try { return new(scalarValue); }
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<NonZeroFloat>(scalarValue); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonZeroFloat>(defect, scalarValue); }
	}
}
