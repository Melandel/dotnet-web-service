namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypedListConverters;

class TypedListToListConverter : TypedListConverter
{
	public static readonly TypedListToListConverter Instance = new();
	TypedListToListConverter()
	{
	}
	public override object Convert(dynamic typedList, Type typedListElementType, Type targetType)
	=> typedList;
}
