namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

public static class TypeExtensionMethods
{
	public static bool IsEqualOrSubclassOf(this Type type, Type targetType)
	=> type == targetType || type.IsSubclassOf(targetType);
}
