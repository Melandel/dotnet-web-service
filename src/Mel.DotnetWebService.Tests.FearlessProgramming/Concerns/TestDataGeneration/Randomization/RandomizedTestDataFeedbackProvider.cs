using System.Collections.Concurrent;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization;

static class RandomizedTestDataFeedbackProvider
{
	static readonly ConcurrentDictionary<string, ConcurrentQueue<RandomizedTestDataFeedback>> Feedbacks = [];
	public static void StoreFeedbackInCaseOfTestFailure(RandomizedTestDataFeedback feedback)
	{
		if(Feedbacks.TryGetValue(feedback.TestId, out var feedbacks))
		{
			feedbacks.Enqueue(feedback);
		}
		else
		{
			Feedbacks.GetOrAdd(
				feedback.TestId,
				_ => {
					var feedbacks = new ConcurrentQueue<RandomizedTestDataFeedback>();
					feedbacks.Enqueue(feedback);
					return feedbacks;
				});
		}
	}

	public static void ForgetFeedbackTiedTo(string testID)
	=> Feedbacks.TryRemove(testID, out _);

	public static bool IsEmpty
	=> Feedbacks.IsEmpty;

	public static string Read(string testId)
	=> string.Join($"{Environment.NewLine}{Environment.NewLine}", Feedbacks[testId].Select(testFeedback => testFeedback.RegenerationHint));
}
