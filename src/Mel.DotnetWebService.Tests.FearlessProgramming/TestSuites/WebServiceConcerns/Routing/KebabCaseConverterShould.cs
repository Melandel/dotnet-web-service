using Mel.DotnetWebService.Api.Concerns.Routing.RouteNamingConvention;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.Routing;

class KebabCaseConverterShould
{
	[TestCase(null, null)]
	[TestCase("", "")]
	[TestCase(" ", " ")]
	[TestCase("  ", "  ")]
	[TestCase("i", "i")]
	[TestCase("I", "i")]
	[TestCase("IO", "io")]
	[TestCase("FileIO", "file-io")]
	[TestCase("SignalR", "signal-r")]
	[TestCase("IOStream", "io-stream")]
	[TestCase("COMObject", "com-object")]
	[TestCase("WebAPI", "web-api")]
	[TestCase("Windows10", "windows-10")]
	[TestCase("WindowsServer2016R2", "windows-server-2016-r2")]
	[TestCase("ApiV1AndApiV2", "api-v1-and-api-v2")]
	public void Convert_String_Into_Hypen_Separated_Words(string input, string expectedOutput)
	{
		// Arrange
		var converter = new KebabCaseConverter();

		// Act
		var actualOutput = converter.Convert(input);

		// Assert
		Assert.That(actualOutput, Is.EqualTo(expectedOutput));
	}
}
