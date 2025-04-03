﻿namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

internal class TestFriendlyHttpClient
{
	readonly HttpClient _httpClient;
	readonly List<HttpResponseMessage> _httpResponseMessagesInvolvedCurrentTest;
	readonly Func<HttpResponseMessage, HttpResponseMessage> _keepTraceOf;
	TestFriendlyHttpClient(
		HttpClient httpClient,
		List<HttpResponseMessage> httpResponseMessagesInvolvedInCurrentTest)
	{
		_httpClient = httpClient;
		_httpResponseMessagesInvolvedCurrentTest = httpResponseMessagesInvolvedInCurrentTest;
		_keepTraceOf = httpResponseMessage => { _httpResponseMessagesInvolvedCurrentTest.Add(httpResponseMessage); return httpResponseMessage; };
	}

	public static TestFriendlyHttpClient ThatStoresHttpResponseMessages(HttpClient httpClient, List<HttpResponseMessage> httpResponseMessagesInvolvedInCurrentTest)
	{
		return new(httpClient, httpResponseMessagesInvolvedInCurrentTest);
	}

	public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)                                         => _keepTraceOf(await _httpClient.DeleteAsync(requestUri));
	public async Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken) => _keepTraceOf(await _httpClient.DeleteAsync(requestUri, cancellationToken));
	public async Task<HttpResponseMessage> DeleteAsync(string requestUri)                                      => _keepTraceOf(await _httpClient.DeleteAsync(requestUri));
	public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)    => _keepTraceOf(await _httpClient.DeleteAsync(requestUri, cancellationToken));

	public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)                                      => _keepTraceOf(await _httpClient.GetAsync(requestUri, completionOption));
	public async Task<HttpResponseMessage> GetAsync(string requestUri)                                                                             => _keepTraceOf(await _httpClient.GetAsync(requestUri));
	public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)    => _keepTraceOf(await _httpClient.GetAsync(requestUri, completionOption, cancellationToken));
	public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)                                         => _keepTraceOf(await _httpClient.GetAsync(requestUri, completionOption));
	public async Task<HttpResponseMessage> GetAsync(Uri requestUri)                                                                                => _keepTraceOf(await _httpClient.GetAsync(requestUri));
	public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)                                        => _keepTraceOf(await _httpClient.GetAsync(requestUri, cancellationToken));
	public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => _keepTraceOf(await _httpClient.GetAsync(requestUri, completionOption, cancellationToken));
	public async Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)                                           => _keepTraceOf(await _httpClient.GetAsync(requestUri, cancellationToken));

	public async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) => _keepTraceOf(await _httpClient.PatchAsync(requestUri, content, cancellationToken));
	public async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)                                      => _keepTraceOf(await _httpClient.PatchAsync(requestUri, content));
	public async Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content)                                         => _keepTraceOf(await _httpClient.PatchAsync(requestUri, content));
	public async Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)    => _keepTraceOf(await _httpClient.PatchAsync(requestUri, content, cancellationToken));

	public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)                                      => _keepTraceOf(await _httpClient.PostAsync(requestUri, content));
	public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) => _keepTraceOf(await _httpClient.PostAsync(requestUri, content, cancellationToken));
	public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)                                         => _keepTraceOf(await _httpClient.PostAsync(requestUri, content));
	public async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)    => _keepTraceOf(await _httpClient.PostAsync(requestUri, content, cancellationToken));

	public async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)                                         => _keepTraceOf(await _httpClient.PutAsync(requestUri, content));
	public async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)    => _keepTraceOf(await _httpClient.PutAsync(requestUri, content, cancellationToken));
	public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)                                      => _keepTraceOf(await _httpClient.PutAsync(requestUri, content));
	public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) => _keepTraceOf(await _httpClient.PutAsync(requestUri, content));

	public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) => _keepTraceOf(_httpClient.Send(request, completionOption, cancellationToken));
	public HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)                                        => _keepTraceOf(_httpClient.Send(request, cancellationToken));
	public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption)                                      => _keepTraceOf(_httpClient.Send(request, completionOption));
	public HttpResponseMessage Send(HttpRequestMessage request)                                                                             => _keepTraceOf(_httpClient.Send(request));

	public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)                                        => _keepTraceOf(await _httpClient.SendAsync(request, cancellationToken));
	public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)                                                                             => _keepTraceOf(await _httpClient.SendAsync(request));
	public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)                                      => _keepTraceOf(await _httpClient.SendAsync(request));
	public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) => _keepTraceOf(await _httpClient.SendAsync(request, cancellationToken));
}
