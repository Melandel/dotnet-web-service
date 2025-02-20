namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypedListConverters;

class TypedListToHashSetConverter : TypedListConverter
{
	public static readonly TypedListToHashSetConverter Instance = new();
	TypedListToHashSetConverter()
	{
	}
	public override object Convert(dynamic typedList, Type typedListElementType, Type targetType)
	{
		var hashSetType = typeof(HashSet<>).MakeGenericType(typedListElementType);
		dynamic hashSetInstance = hashSetType
			.GetConstructor(new[] { typeof(IEnumerable<>).MakeGenericType(typedListElementType) })
			!.Invoke(new object[] { typedList });
		return hashSetInstance;
	}
}
