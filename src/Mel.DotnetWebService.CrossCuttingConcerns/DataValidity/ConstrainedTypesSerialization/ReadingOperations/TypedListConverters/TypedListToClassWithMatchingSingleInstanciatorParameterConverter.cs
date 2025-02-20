using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypedListConverters;

class TypedListToClassWithMatchingSingleInstanciatorParameterConverter : TypedListConverter
{
	public static readonly TypedListToClassWithMatchingSingleInstanciatorParameterConverter Instance = new();
	TypedListToClassWithMatchingSingleInstanciatorParameterConverter()
	{
	}

	public override object Convert(dynamic typedList, Type typedListElementType, Type targetType)
	{
		object array = TypedListToArrayConverter.Instance.Convert(typedList, typedListElementType, targetType);
		var classWithMatchingSingleInstanciatorParameter = targetType.CreateInstanceUsingConstructorOrFactoryMethod(array, BindingFlags.Public);
		return classWithMatchingSingleInstanciatorParameter;
	}
}
