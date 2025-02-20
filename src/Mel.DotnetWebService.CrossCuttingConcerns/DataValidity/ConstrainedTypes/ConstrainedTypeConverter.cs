namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

using System.ComponentModel;
using System.Globalization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

public class ConstrainedTypeConverter : TypeConverter
{
	readonly Type _concreteType;
	public ConstrainedTypeConverter(Type concreteType)
	{
		_concreteType = concreteType;
	}
	public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
	=> destinationType != null && ConstrainedTypeInfos.Include(destinationType);

	public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
	{
		if (value == null)
		{
			return null;
		}

		if (ConstrainedTypeInfos.TryGet(destinationType, out var constrainedTypeInfo))
		{
			return constrainedTypeInfo.InvokeImplicitConversionToFullyNativeType(value);
		}

		throw new NotImplementedException();
	}

	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
	=> IsAbleToConvertFrom(sourceType, _concreteType);

	bool IsAbleToConvertFrom(Type sourceType, Type concreteType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}

		if (!ConstrainedTypeInfos.TryGet(_concreteType, out var constrainedTypeInfo))
		{
			return false;
		}

		if (constrainedTypeInfo.FullyNativeRootType.IsAssignableFrom(sourceType))
		{
			return true;
		}

		if (sourceType == typeof(string))
		{
			return constrainedTypeInfo.FullyNativeRootType.IsSerializedIntoString();
		}

		if (IsCollectionType(constrainedTypeInfo.FullyNativeRootType) && IsCollectionType(sourceType))
		{
			return IsAbleToConvertFrom(
				sourceType.GetCollectionItemType(),
				constrainedTypeInfo.FullyNativeRootType.GetCollectionItemType());
		}

		return false;
	}

	bool IsCollectionType(Type type)
		=> type.IsArray || type.IsOrImplementsGenericInterface(typeof(IEnumerable<>));

	public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	{
		if (!ConstrainedTypeInfos.TryGet(_concreteType, out var constrainedTypeInfo))
		{
			throw new NotImplementedException();
		}

		var valueType = value.GetType();
		if (valueType == constrainedTypeInfo.FullyNativeRootType)
		{
			return constrainedTypeInfo.InvokeStaticFactoryMethod(value);
		}

		object valueAsSupportedRootType = value is string valueAsString
			? ConvertStringToOtherType(valueAsString, constrainedTypeInfo.FullyNativeRootType)
			: Convert.ChangeType(value, constrainedTypeInfo.FullyNativeRootType, CultureInfo.InvariantCulture);

		return constrainedTypeInfo.InvokeStaticFactoryMethod(valueAsSupportedRootType);
	}
	public static object ConvertStringToOtherType(string value, Type targetType)
	{
		targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
		if (targetType == typeof(string))
		{
			return value;
		}

		var converter = TypeDescriptor.GetConverter(targetType);
		if (converter.CanConvertFrom(typeof(string)))
		{
			return converter.ConvertFromInvariantString(value)!;
		}

		return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
	}
}
