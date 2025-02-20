namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypedListConverters;

class NotImplementedTypedListConverter : TypedListConverter
{
	public static readonly NotImplementedTypedListConverter Instance = new();
	NotImplementedTypedListConverter()
	{
	}

	public override object Convert(dynamic typedList, Type typedListElementType, Type targetType)
	=> throw new NotImplementedException(
		$"None of the existing {nameof(TypedListConverter)}s can .{nameof(Convert)} type {targetType.GetName()}. Supported types inclue arrays, interfaces provided by the System namespace (for instance IEnumerable<>), List<>, HashSet<>, and objects with a single collection parameter in a public constructor or factory method");
}
