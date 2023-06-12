using Newtonsoft.Json;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

static class HttpResponseMessageExtensionMethods
{
	public async static Task<T> ToResponseObject<T>(this HttpResponseMessage httpResponseMessage)
	=> JsonConvert.DeserializeObject<T>(
		await httpResponseMessage.GetContentAsString(),
		(Newtonsoft.Json.JsonConverter)new FrameworkExtension.Serialization.JsonConverter.That_Does_Not_Ignore_ProblemDetails_Extensions_Property());

	public async static Task<string> GetContentAsString(this HttpResponseMessage httpResponseMessage)
	=> await httpResponseMessage.Content.ReadAsStringAsync();
}
