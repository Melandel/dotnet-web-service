using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

public static class DelegatingHandlerTestDouble
{
	public static DelegatingHandler That_Writes_HttpResponse_In_TestOutput
	=> new DelegatingHandlerThatWritesHttpResponseInTestOutput();

	class DelegatingHandlerThatWritesHttpResponseInTestOutput : DelegatingHandler
	{
		protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var httpResponseMessage = await base.SendAsync(request, cancellationToken);

			NUnit.Framework.TestContext.WriteLine(await BuildMessageInTestOutput(httpResponseMessage));
			return httpResponseMessage;
		}

		static async Task<string> BuildMessageInTestOutput(HttpResponseMessage httpResponseMessage)
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
}
