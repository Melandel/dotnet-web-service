namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class StringExtensionMethods
{
	public static bool Matches(this string? left, string? right, bool caseSensitive = false)
	=> caseSensitive
		? string.Equals(left, right)
		: string.Equals(left, right, StringComparison.InvariantCultureIgnoreCase);
}

