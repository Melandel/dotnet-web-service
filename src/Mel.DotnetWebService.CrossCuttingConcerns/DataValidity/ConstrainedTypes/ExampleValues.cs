namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public class ExampleValues<T> where T : notnull
{
	public ArrayOfUniqueValuesWithAtLeast2Items<T> ValidValues { get; }
	public T SomeValue => ((T[])ValidValues)[0];
	public T SomeOtherValue => ((T[])ValidValues)[1];

	public Dictionary<T, string> ErrorMessagesByInvalidValue { get; }

	ExampleValues(ArrayOfUniqueValuesWithAtLeast2Items<T> validValues, Dictionary<T, string> errorMessagesByInvalidValue)
	{
		ValidValues = validValues;
		ErrorMessagesByInvalidValue = errorMessagesByInvalidValue;
	}

	public static ExampleValues<T> ValidAndInvalid(IEnumerable<T> validValues, IDictionary<T, string> errorMessagesByInvalidValue)
	{
		try
		{
			return new(
				ArrayOfUniqueValuesWithAtLeast2Items.ApplyConstraintsTo(validValues),
				errorMessagesByInvalidValue.ToDictionary());
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(validValues, errorMessagesByInvalidValue); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, validValues, errorMessagesByInvalidValue); }
	}
}

public static class ExampleValues
{
	public static ExampleValues<T> ValidAndInvalid<T>(IEnumerable<T> validValues, IDictionary<T, string> errorMessagesByInvalidValue)
		where T : notnull
	=> ExampleValues<T>.ValidAndInvalid(validValues, errorMessagesByInvalidValue);
}
