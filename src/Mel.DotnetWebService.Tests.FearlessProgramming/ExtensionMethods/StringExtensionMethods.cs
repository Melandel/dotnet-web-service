using System.Text;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

public static class StringExtensionMethods
{
	public static HttpContent ToJsonContent(this string payloadAsString)
	=> new StringContent(payloadAsString, Encoding.UTF8, "application/json");

	public static string WithFirstCharacterInLowerCase(this string str)
	=> str switch
	{
		null => null,
		"" => "",
		[var c] => $"{char.ToLower(c)}",
		_ => $"{char.ToLower(str[0])}{str.Substring(1)}"
	};
}

