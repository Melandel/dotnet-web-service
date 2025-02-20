using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

static class PropertyInfoExtensionMethods
{
	public static bool HasAPublicGetter(this PropertyInfo propertyInfo, out MethodInfo getter)
	{
		var publicAccessors = propertyInfo.GetAccessors(nonPublic: false);
		var publicGetters = publicAccessors.Where(method => method.ReturnType != typeof(void));

		if (!publicGetters.Any())
		{
			getter = null!;
			return false;
		}

		getter = publicGetters.First();
		return true;
	}

	public static bool IsInitOnly(this PropertyInfo property)
	{
		if (!property.CanWrite)
		{
			return false;
		}

		var setMethod = property.SetMethod!;

		var setMethodReturnParameterModifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();

		return setMethodReturnParameterModifiers.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit));
	}
}
