namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

public static class TypeExtensionMethods
{
	public static bool IsDefinedByOurOrganization(this Type type)
	=> type.Namespace is not null && type.Namespace.StartsWith(StringArchetype.CurrentNamespacePrefix(2));

	public static bool IsEqualOrSubclassOf(this Type type, Type targetType)
	=> type == targetType || type.IsSubclassOf(targetType);
}

