using Mel.DotnetWebService.Api.ErrorHandling;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

static class ExceptionExtensionMethods
{
	internal static HttpProblem ToProblem(this Exception exception, Controllers.HttpProblemTypeProvider httpProblemTypeProvider)
	=> HttpProblem.From(
		httpProblemType: exception switch
		{
			EnumValueReceivedFromIntegerException ex => httpProblemTypeProvider.GetEnumValueReceivedFromInteger(),
			_ => httpProblemTypeProvider.GetDeveloperMistake()
		},
		httpProblemOccurrence: exception switch
		{
			_ => HttpProblemOccurrence.FromIdAndExplanationThatShouldHelpTheClientUnderstandTheApi(
				new Uri("https://www.google.com"),
				"foo",
				new Dictionary<DebuggingInformationName, object>
				{
					{ DebuggingInformationName.StackTrace, exception.StackTrace! }
				})
		});
}
