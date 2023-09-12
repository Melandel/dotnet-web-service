using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ExceptionExtensionMethods
{
	public static bool TryConversionToHttpProblem(this Exception exception, HttpContext httpContext, out HttpProblem httpProblem)
	=> Concerns.ErrorHandling.Integration.ExceptionProcessing.TryConvertingExceptionToHttpProblem(exception, httpContext, out httpProblem);

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

		return string.Join(", ", exceptionsTypes);
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
