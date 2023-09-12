using Mel.DotnetWebService.Api.Concerns.ErrorHandling;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.HttpProblemTypes;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ExceptionExtensionMethods
{
	public static HttpProblem ToProblem(this Exception exception, HttpContext httpContext)
	{
		var httpProblemType = BuildHttpProblemType(
			exception,
			httpContext.RequestServices.GetRequiredService<Controllers.HttpProblemTypeProvider>());

		var httpProblemOccurrence = BuildHttpProblemOccurrence(
			BuildProblemOccurrenceId(httpContext),
			httpProblemType,
			exception);

		return HttpProblem.From(httpProblemType, httpProblemOccurrence);
	}

	static HttpProblemType BuildHttpProblemType(Exception exception, Controllers.HttpProblemTypeProvider httpProblemTypeProvider)
	=> exception switch
	{
		EnumValueReceivedFromIntegerException => httpProblemTypeProvider.GetEnumValueReceivedFromInteger(),
		_ => httpProblemTypeProvider.GetDeveloperMistake()
	};

	static HttpProblemOccurrence BuildHttpProblemOccurrence(Uri problemOccurrenceId, HttpProblemType httpProblemType, Exception exception)
	=> (httpProblemType, exception) switch
	{
		(EnumValueReceivedFromInteger problemType, EnumValueReceivedFromIntegerException ex) => HttpProblemOccurrence.FromIncorrectApiUsage(
			id: problemOccurrenceId,
			statementDescribingIncorrectApiUsage: $"Parameter \"{ex.ParameterName}\" does not accept value {ex.IntegerValue}.",
			statementHelpingApiUserTowardsCorrectApiUsage: $"Supported values: {string.Join(", ", ex.SupportedEnumValues)}.",
			problemSpecificInformation: problemType.DefiningSpecification.ExpectedExtensionMembers
				.ToDictionary<HttpProblemTypeExtensionMember, HttpProblemTypeExtensionMember, object>(
					keySelector: extensionMemberName => extensionMemberName,
					elementSelector: extensionMemberName => extensionMemberName switch
					{
						HttpProblemTypeExtensionMember.IntegerValue => ex.IntegerValue,
						HttpProblemTypeExtensionMember.EnumParameterName => ex.ParameterName,
						HttpProblemTypeExtensionMember.SupportedEnumValues => ex.SupportedEnumValues,
						var extensionMemberRequiredButMissing => throw MissingProblemTypeExtensionMemberException.WhenBuildingHttpProblemOccurrence(problemType.GetType(), extensionMemberRequiredButMissing)
					})),
		(var problemType, var ex) => HttpProblemOccurrence.FromDeveloperMistakeInThisApi(
			id: problemOccurrenceId,
			apologeticStatement: "Oops! Something wrong happened on our side...",
			problemSpecificInformation: problemType.DefiningSpecification.ExpectedExtensionMembers
				.ToDictionary<HttpProblemTypeExtensionMember, HttpProblemTypeExtensionMember, object>(
					keySelector: extensionMemberName => extensionMemberName,
					elementSelector: extensionMemberName => extensionMemberName switch
					{
						HttpProblemTypeExtensionMember.StackTrace => ex.GetStackTraceFromAllInnerExceptions(),
						HttpProblemTypeExtensionMember.ExceptionsTypes => ex.GetConcatenatedExceptionTypesFromAllInnerExceptions(),
						HttpProblemTypeExtensionMember.ExceptionsAggregatedMessages => ex.GetConcatenatedMessagesFromAllInnerExceptions(),
						HttpProblemTypeExtensionMember.ExceptionsAggregatedData => ex.GetConcatenatedDataFromAllInnerExceptions(),
						var extensionMemberRequiredButMissing => throw MissingProblemTypeExtensionMemberException.WhenBuildingHttpProblemOccurrence(problemType.GetType(), extensionMemberRequiredButMissing)
					}))
	};

	static Uri BuildProblemOccurrenceId(HttpContext httpContext)
	{
		var baseApiUrl = new Uri($"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/");
		var someFormOfRequestId = System.Diagnostics.Activity.Current?.Id ?? httpContext.TraceIdentifier;
		return new Uri(baseApiUrl, someFormOfRequestId);
	}

	public static string GetStackTraceFromAllInnerExceptions(this Exception exception)
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

	public static string GetConcatenatedExceptionTypesFromAllInnerExceptions(this Exception exception)
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

	public static string GetConcatenatedMessagesFromAllInnerExceptions(this Exception exception)
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

	public static Dictionary<object, object?> GetConcatenatedDataFromAllInnerExceptions(this Exception exception)
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
