using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypedListConverters;

class TypedListToConstrainedGenericCollectionConverter : TypedListConverter
{
	public static readonly TypedListToConstrainedGenericCollectionConverter Instance = new();
	TypedListToConstrainedGenericCollectionConverter()
	{
	}
	public override object Convert(dynamic typedList, Type typedListElementType, Type targetType)
	=> ConstrainedTypeInfos.ReconstituteFromRootTypeValue(targetType, typedList);
}
