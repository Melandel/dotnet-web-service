using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypedListConverters;

class TypedListToArrayConverter : TypedListConverter
{
	public static readonly TypedListToArrayConverter Instance = new();
	TypedListToArrayConverter()
	{
	}

	public override object Convert(dynamic typedList, Type typedListElementType, Type targetType)
	=> typedList.GetType().InvokeMember(
		nameof(List<object>.ToArray),
		BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
		null,
		typedList,
		Array.Empty<object>());
}
