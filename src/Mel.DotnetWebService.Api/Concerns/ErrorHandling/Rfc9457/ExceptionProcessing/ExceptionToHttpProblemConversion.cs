using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ExceptionProcessing;

static class ExceptionToHttpProblemConversion
{
	public static bool TryConverting(Exception exception, HttpContext httpContext, out HttpProblem httpProblem)
	{
		try
		{
			var baseApiUrl = new Uri($"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/");
			var someFormOfRequestId = System.Diagnostics.Activity.Current?.Id ?? httpContext.TraceIdentifier;
			var someFormOfIdentifierForTheProblemOccurrence = new Uri(baseApiUrl, someFormOfRequestId);

			var httpProblemTypeProvider = httpContext.RequestServices.GetRequiredService<Controllers.HttpProblemTypeProvider>();

			switch (exception)
			{
				case EnumValueReceivedFromIntegerException ex:
					httpProblem = EnumsHandling.Integration.ErrorHandling.Convert(ex, someFormOfIdentifierForTheProblemOccurrence, httpProblemTypeProvider);
					return true;
				default:
					httpProblem = HttpProblem.FromDeveloperMistake(
						exception,
						httpProblemTypeProvider.GetDeveloperMistake(),
						someFormOfIdentifierForTheProblemOccurrence);
					return true;
			}
		}
		catch
		{
			httpProblem = null!;
			return false;
		}
	}
}
