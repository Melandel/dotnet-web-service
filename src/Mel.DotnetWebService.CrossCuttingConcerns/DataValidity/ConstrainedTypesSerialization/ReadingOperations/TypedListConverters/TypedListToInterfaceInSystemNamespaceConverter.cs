namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypedListConverters;

class TypedListToInterfaceInSystemNamespaceConverter : TypedListConverter
{
	public static readonly TypedListToInterfaceInSystemNamespaceConverter Instance = new();
	TypedListToInterfaceInSystemNamespaceConverter()
	{
	}
	public override object Convert(dynamic typedList, Type typedListElementType, Type targetType)
	=> typedList;
}
