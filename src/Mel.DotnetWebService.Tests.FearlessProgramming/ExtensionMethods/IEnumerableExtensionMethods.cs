using System.Collections.Concurrent;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

static class IEnumerableExtensionMethods
{
	public static void ParallelForEach<T>(this IEnumerable<T> testInputs, Action<T> testExecution)
	{
		var testInputsFailingFromAssertion = new ConcurrentBag<T>();
		var testInputsFailingFromException = new ConcurrentBag<T>();

		Parallel.ForEach(testInputs, testInput =>
		{
			try { testExecution.Invoke(testInput); }
			catch (AssertionException) { testInputsFailingFromAssertion.Add(testInput); }
			catch                      { testInputsFailingFromException.Add(testInput); }
		});

		if (testInputsFailingFromAssertion.IsEmpty && testInputsFailingFromException.IsEmpty)
		{
			return;
		}

		using (Assert.EnterMultipleScope())
		{
			foreach (var testInputFailingFromAssertion in testInputsFailingFromAssertion)
			{
				testExecution.Invoke(testInputFailingFromAssertion);
			}
			foreach (var testInputFailingFromException in testInputsFailingFromException)
			{
				testExecution.Invoke(testInputFailingFromException);
			}
		}
	}
}
