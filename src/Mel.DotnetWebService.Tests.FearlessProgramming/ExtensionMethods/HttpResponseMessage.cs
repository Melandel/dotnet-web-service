using Newtonsoft.Json;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

public static class HttpResponseMessageExtensionMethods
{
	public async static Task<T> ToResponseObject<T>(this HttpResponseMessage httpResponseMessage)
	=> JsonConvert.DeserializeObject<T>(await httpResponseMessage.GetContentAsString());
	public async static Task<string> GetContentAsString(this HttpResponseMessage httpResponseMessage)
	=> new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync()).ReadToEnd();
}
