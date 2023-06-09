﻿using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.ErrorHandling;

public class ExceptionHandlingOutput
{
	public string ContentType { get; }
	public int StatusCode => ProblemDetails.Status!.Value;
	public ProblemDetails ProblemDetails { get; }
	ExceptionHandlingOutput(string contentType, ProblemDetails problemDetails)
	{
		ContentType = contentType;
		ProblemDetails = problemDetails;
	}

	public static ExceptionHandlingOutput From(HttpContext httpContext, Exception ex)
	{
		var instance = new ExceptionHandlingOutput(
			contentType: "application/json",
			problemDetails: new ProblemDetails
			{
				Title = @"
					A short, human-readable summary of the problem type.
					It SHOULD NOT change from occurrence to occurrence of the problem, except for purposes of localization (e.g., using proactive content negotiation; see [RFC7231], Section 3.4).
					Consumers MUST use the ""type"" string as the primary identifier for the problem type; the ""title"" string is advisory and included only for users who are not aware of the semantics of the URI and do not have the ability to discover them (e.g., offline log analysis).
					Consumers SHOULD NOT automatically dereference the type URI.",
				Detail = @"
					A human-readable explanation specific to this occurrence of the problem.
					The ""detail"" member, if present, ought to focus on helping the client correct the problem, rather than giving debugging information.
					Consumers SHOULD NOT parse the ""detail"" member for information; extensions are more suitable and less error-prone ways to obtain such information.",
				Instance = @"
					A URI reference that identifies the specific occurrence of the problem.
					It may or may not yield further information if dereferenced.",
				Status = (int) HttpStatusCode.InternalServerError,
					// The HTTP status code ([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.
					// The "status" member, if present, is only advisory; it conveys the HTTP status code used for the convenience of the consumer.
				Type = @"
					A URI reference [RFC3986] that identifies the problem type.
					This specification encourages that, when dereferenced, it provide human-readable documentation for the problem type (e.g., using HTML [W3C.REC-html5-20141028]).
					When this member is not present, its value is assumed to be ""about:blank"".Consumers MUST use the ""type"" string as the primary identifier for the problem type; the ""title"" string is advisory and included only for users who are not aware of the semantics of the URI and do not have the ability to discover them (e.g., offline log analysis).
					Consumers SHOULD NOT automatically dereference the type URI."
			});

		var extensions = instance.ProblemDetails.Extensions;
		extensions.Add(
			"a word that is an extension of the problem. For instance, a \"out of credit\" problem may define a \"balance\" extension.",
			"additional, problem-specific information");
		extensions.Add(
			"a word that is an extension of the problem. For instance, a \"out of credit\" problem may define a \"accounts\" extension.",
			"additional, problem-specific information");
		extensions.Add(
			"Clients consuming problem details MUST ignore any such extensions that they don't recognize; this allows problem types to evolve and include additional information in the future.",
			"additional, problem-specific information");

		return instance;
	}
}
