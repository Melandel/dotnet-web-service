﻿using System.Collections.ObjectModel;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling;

class HttpProblemOccurrence
{
	public Uri? Id { get; }
	public Statement FeedbackForApiUser { get; }
	public IReadOnlyDictionary<HttpProblemTypeExtensionMember, object> ProblemSpecificInformation { get; }
	HttpProblemOccurrence(
		Uri? id,
		Statement feedbackForApiUser,
		ReadOnlyDictionary<HttpProblemTypeExtensionMember, object> problemSpecificInformation)
	{
		Id = id;
		FeedbackForApiUser = feedbackForApiUser;
		ProblemSpecificInformation = problemSpecificInformation;
	}

	public static HttpProblemOccurrence FromIncorrectApiUsage(
		Uri id,
		string statementDescribingIncorrectApiUsage,
		string? statementHelpingApiUserTowardsCorrectApiUsage,
		IDictionary<HttpProblemTypeExtensionMember, object> problemSpecificInformation)
	=> new(
		id,
		(statementDescribingIncorrectApiUsage, statementHelpingApiUserTowardsCorrectApiUsage) switch
		{
			(_, null or "") => Statement.FromString(statementDescribingIncorrectApiUsage),
			_ => Statement.FromString(statementDescribingIncorrectApiUsage) + Statement.FromString(statementHelpingApiUserTowardsCorrectApiUsage)
		},
		problemSpecificInformation.AsReadOnly());

	public static HttpProblemOccurrence FromDeveloperMistakeInThisApi(
		Uri id,
		string apologeticStatement,
		IDictionary<HttpProblemTypeExtensionMember, object> problemSpecificInformation)
	=> new(
		id,
		Statement.FromString(apologeticStatement),
		problemSpecificInformation.AsReadOnly());
}
