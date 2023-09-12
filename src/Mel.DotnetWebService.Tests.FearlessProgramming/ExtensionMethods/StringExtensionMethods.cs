using System.Text;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

public static class StringExtensionMethods
{
	public static HttpContent ToJsonContent(this string payloadAsString)
	=> new StringContent(payloadAsString, Encoding.UTF8, "application/json");

}
