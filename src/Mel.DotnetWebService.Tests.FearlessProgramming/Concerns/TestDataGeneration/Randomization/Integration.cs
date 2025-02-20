using System.Text;
using NUnit.Framework.Interfaces;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization;

static class Integration
{
	public static void LogHintOnFailingTest()
	{
		if (TestContext.CurrentContext.Test.Properties.ContainsKey(nameof(RetryAttribute)))
		{
			return;
		}

		if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
		{
			TestContext.Progress.WriteLine(
				BuildFailedTestUsingRandomizedDataFeedback(TestContext.CurrentContext.Test.ID));
		}

		RandomizedTestDataFeedbackProvider.ForgetFeedbackTiedTo(TestContext.CurrentContext.Test.ID);
	}

	static string BuildFailedTestUsingRandomizedDataFeedback(string testId)
	=> new StringBuilder(3 * TestContext.CurrentContext.Test.FullName.Length)
		.AppendLine()
		.AppendLine($"{TestContext.CurrentContext.Test.FullName}")
		.AppendLine(new string('=', TestContext.CurrentContext.Test.FullName.Length))
		.AppendLine($"Failed using the following {nameof(Randomized)} Test Data")
		.AppendLine()
		.AppendLine(RandomizedTestDataFeedbackProvider.Read(testId).IndentWith(new string(' ', 2)))
		.AppendLine()
		.ToString();
}
