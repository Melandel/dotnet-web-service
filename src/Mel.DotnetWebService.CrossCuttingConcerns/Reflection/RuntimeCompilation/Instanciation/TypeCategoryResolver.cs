using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

class TypeCategoryResolver
{
	public static TypeCategory Resolve(Type type)
	{
		if (type.IsEnum)
		{
			return TypeCategory.EnumType;
		}

		if (type.IsANativeScalarType())
		{
			return TypeCategory.NativeValueType;
		}

		if (type.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes))
		{
			var firstArgType = argTypes.First();
			return (firstArgType.IsGenericType && firstArgType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
				? TypeCategory.UnconstrainedCollectionOfKeyValuePairsType
				: TypeCategory.CollectionType;
		}

		return TypeCategory.ParentObjectType;
	}
}
