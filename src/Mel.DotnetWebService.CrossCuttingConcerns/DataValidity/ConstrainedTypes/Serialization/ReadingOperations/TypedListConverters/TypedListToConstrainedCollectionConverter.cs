using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypedListConverters;

class TypedListToConstrainedCollectionConverter : TypedListConverter
{
	public static TypedListToConstrainedCollectionConverter Instance = new();
	TypedListToConstrainedCollectionConverter()
	{
	}
	public override object Convert(dynamic typedList, Type typedListElementType, Type targetType)
	{
		if (ConstrainedTypeInfos.TryGet(targetType, out var constrainedTypeInfo))
		{
			return constrainedTypeInfo.InvokeStaticFactoryMethod(typedList);
		}

		if (targetType.IsGenericType)
		{
			var genericTypeDefinition = targetType.GetGenericTypeDefinition();
			return Convert(typedList, typedListElementType, genericTypeDefinition);
		}

		throw new NotImplementedException("foo");
	}
}
