using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

public class ExampleValues<T>
{
	public ArrayOfUniqueValuesWithAtLeast2Items<T> ValidValues { get; }
	public NonEmptyArray<ConstraintViolationExample<T>> ConstraintViolationExamples { get; }

	ExampleValues(ArrayOfUniqueValuesWithAtLeast2Items<T> validValues, NonEmptyArray<ConstraintViolationExample<T>> constraintViolationExamples)
	{
		ValidValues = validValues;
		ConstraintViolationExamples = constraintViolationExamples;
	}

	public static ExampleValues<T> ValidAndInvalid(IEnumerable<T> validValues, IEnumerable<ConstraintViolationExample<T>> constraintViolationExamples)
	{
		try
		{
			return new(
				ArrayOfUniqueValuesWithAtLeast2Items.ApplyConstraintsTo(validValues),
				NonEmptyArray<ConstraintViolationExample<T>>.ApplyConstraintsTo(constraintViolationExamples));
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyHashSet<T>>(validValues, constraintViolationExamples); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyHashSet<T>>(defect, validValues, constraintViolationExamples); }
	}
}
