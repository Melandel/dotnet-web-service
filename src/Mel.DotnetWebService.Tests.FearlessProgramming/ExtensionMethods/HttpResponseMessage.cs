namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

public static class HttpResponseMessageExtensionMethods
{
	public async static Task<string> GetContentAsString(this HttpResponseMessage httpResponseMessage)
	=> new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync()).ReadToEnd();
}
