using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

static class PropertyInfoExtensionMethods
{
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
