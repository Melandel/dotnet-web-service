namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

public static class HttpResponseMessageExtensionMethods
{
	public async static Task<string> GetContentAsString(this HttpResponseMessage httpResponseMessage)
	=> await httpResponseMessage.Content.ReadAsStringAsync();
}

