using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

public static class HttpResponseMessageExtensionMethods
{
	public async static Task<string> GetContentAsString(this HttpResponseMessage httpResponseMessage)
	=> await httpResponseMessage.Content.ReadAsStringAsync();

	public static async Task<string> Render(this HttpResponseMessage httpResponseMessage)
	{
		var responseStatus = $"{(int)httpResponseMessage.StatusCode} {httpResponseMessage.ReasonPhrase}";
		var separator = new string('-', responseStatus.Length);
		var response = await httpResponseMessage.GetContentAsString();
		var formattedResponse = response switch
		{
			null => "[null]",
			"" => "[empty]",
			_ when string.IsNullOrWhiteSpace(response) => "[whitespace]",
			['{', .., '}'] or ['[', .., ']'] => TryJsonFormattingWithFallbackAsRawString(response),
			_ => response,
		};

		var message = string.Join(Environment.NewLine, new[] { responseStatus, separator, formattedResponse });
		return message;
	}

	static string TryJsonFormattingWithFallbackAsRawString(string response)
	{
		try
		{
			return JValue.Parse(response).ToString(Formatting.Indented);
		}
		catch
		{
			return response;
		}
	}
}

