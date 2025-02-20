using Mel.DotnetWebService.Api.Concerns.ErrorHandling.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling;

static partial class Integration
{
	public static class ExceptionAnalysis
	{
		public static bool TryConvertingExceptionToHttpProblem(Exception exception, HttpContext httpContext, out HttpProblem httpProblem)
		{
			try
			{
				var baseApiUrl = new Uri($"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/");
				var someFormOfRequestId = System.Diagnostics.Activity.Current?.Id ?? httpContext.TraceIdentifier;
				var someFormOfIdentifierForTheProblemOccurrence = new Uri(baseApiUrl, someFormOfRequestId);

				var httpProblemTypeProvider = httpContext.RequestServices.GetRequiredService<Controllers.HttpProblemTypeProvider>();
				var httpProblemType = httpProblemTypeProvider.GetDeveloperMistake();

				var httpProblemOccurrence = HttpProblemOccurrence.FromDeveloperMistakeInThisApi(
					id: someFormOfIdentifierForTheProblemOccurrence,
					apologeticStatement: "Oops! Something wrong happened on our side...",
					problemSpecificInformation: httpProblemType.DefiningSpecification.ExpectedExtensionMembers
						.ToDictionary<HttpProblemTypeExtensionMember, HttpProblemTypeExtensionMember, object>(
							keySelector: extensionMemberName => extensionMemberName,
							elementSelector: extensionMemberName => extensionMemberName switch
							{
								HttpProblemTypeExtensionMember.StackTrace => exception.GetStackTraceFromAllInnerExceptions(),
								HttpProblemTypeExtensionMember.ExceptionsTypes => exception.GetConcatenatedExceptionTypesFromAllInnerExceptions(),
								HttpProblemTypeExtensionMember.ExceptionsAggregatedMessages => exception.GetConcatenatedMessagesFromAllInnerExceptions(),
								HttpProblemTypeExtensionMember.ExceptionsAggregatedData => exception.GetConcatenatedDataFromAllInnerExceptions(),
								var extensionMemberRequiredButMissing => throw new NotImplementedException($"the extension member {extensionMemberRequiredButMissing.ToString()} is not supported when a {httpProblemType.GetType().Name}-type problem occurs.")
							}));

				httpProblem = HttpProblem.From(httpProblemType, httpProblemOccurrence);
				return true;
			}
			catch
			{
				httpProblem = null!;
				return false;
			}
		}

		public static string GetStackTraceFromAllInnerExceptions(Exception exception)
		{
			if (exception is null)
			{
				return string.Empty;
			}

			if (exception.StackTrace is not null)
			{
				return exception.StackTrace;
			}

			var e = exception;
			while (e.InnerException != null)
			{
				if (e.InnerException.StackTrace is not null)
				{
					return e.InnerException.StackTrace;
				}
				e = e.InnerException;
			}

			return string.Empty;
		}

		public static string GetConcatenatedExceptionTypesFromAllInnerExceptions(Exception exception)
		{
			if (exception is null)
			{
				return string.Empty;
			}

			var exceptionsTypes = new List<string>() { exception.GetType().FullName! };
			var e = exception;
			while (e.InnerException != null)
			{
				exceptionsTypes.Add(e.InnerException.GetType().FullName!);
				e = e.InnerException;
			}

			return String.Join(", ", exceptionsTypes);
		}

		public static string GetConcatenatedMessagesFromAllInnerExceptions(Exception exception)
		{
			if (exception is null)
			{
				return string.Empty;
			}

			var aggregatedMessage = new System.Text.StringBuilder(exception.Message);
			var e = exception;
			while (e.InnerException != null)
			{
				aggregatedMessage.AppendLine(e.InnerException.Message);
				e = e.InnerException;
			}

			return aggregatedMessage.ToString();
		}

		public static Dictionary<object, object?> GetConcatenatedDataFromAllInnerExceptions(Exception exception)
		{
			if (exception is null)
			{
				return new Dictionary<object, object?>();
			}

			var aggregatedData = new Dictionary<object, object?>();
			var count = 0;
			foreach (var key in exception.Data.Keys)
			{
				aggregatedData.Add($"{count}:key", exception.Data[key]);
			}

			var e = exception;
			while (e.InnerException != null)
			{
				count++;
				foreach (var key in e.InnerException.Data.Keys)
				{
					aggregatedData.Add($"{count}:key", exception.Data[key]);
				}
				e = e.InnerException;
			}

			return aggregatedData;
		}
	}
}
