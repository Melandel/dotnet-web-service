using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.PerformanceBenchmarks.CrossCuttingConcerns.RuntimeTypeManipulation;

[PerformanceBenchmark]
class GenericListBuilderBenchmarks
{
	[Test]
	public void List_of_int()
	{
		var nbIterations = 1000;
		var nbIterationsIncludingInitIteration = nbIterations+1;
		var dataType = typeof(int);
		var log = Log.ForCurrentTest(dataType, nbIterations);
		var data = new int[nbIterations];
		for (int i = 0; i < nbIterations; i++) { data[i] = i; }
		var someData = Some.ValueAsDynamic(dataType);
		var sw = new Stopwatch();
		var swForAddOperations = new Stopwatch();
		var elapsed = sw.Elapsed;
		var elapsedForAddOperations = swForAddOperations.Elapsed;

		for (int n = 0; n < 2; n++)
		{
			sw = Stopwatch.StartNew(); var list = new List<int>(nbIterationsIncludingInitIteration); list.Add(someData);
			swForAddOperations = Stopwatch.StartNew(); for (int i = 0; i < nbIterations; i++) { list.Add(data[i]); }
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n ==  0) { continue; }
			log.BenchmarkCase = "List<int>.Add";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw = Stopwatch.StartNew(); var list2 = new List<int>(nbIterationsIncludingInitIteration); list2.Add(someData);
			swForAddOperations = Stopwatch.StartNew(); list2.AddRange(data);
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "List<int>.AddRange";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}


		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var listBuilder = GenericListBuilder.For(dataType); listBuilder.Add(someData);
			swForAddOperations.Restart(); for (int i = 0; i < nbIterations; i++) { listBuilder.Add(data[i]); }
			var listBuiltUsingAdd = listBuilder.BuildAsIList();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericListBuilder.Add";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var listBuiltUsingAddRangeBuilder = GenericListBuilder.For(dataType); listBuiltUsingAddRangeBuilder.Add(someData);
			swForAddOperations.Restart(); listBuiltUsingAddRangeBuilder.AddRange(data).BuildAsIList();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericListBuilder.AddRange";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var listBuiltUsingAddRangeBuilderWithKnowCapacity = GenericListBuilder.For(dataType, nbIterationsIncludingInitIteration); listBuiltUsingAddRangeBuilderWithKnowCapacity.Add(someData);
			swForAddOperations.Restart(); listBuiltUsingAddRangeBuilderWithKnowCapacity.AddRange(data).BuildAsIList();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericListBuilder.AddRange+capacity";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}
	}

	[Test]
	public void List_of_NonEmptyGuids()
	{
		var nbIterations = 1000;
		var nbIterationsIncludingInitIteration = nbIterations+1;
		var dataType = typeof(NonEmptyGuid);
		var log = Log.ForCurrentTest(dataType, nbIterations);
		var data = new NonEmptyGuid[nbIterations];
		for (int i = 0; i < nbIterations; i++) { data[i] = NonEmptyGuid.ApplyConstraintsTo(Guid.NewGuid()); }
		var someData = Some.ValueAsDynamic(dataType);
		var sw = new Stopwatch();
		var swForAddOperations = new Stopwatch();
		var elapsed = sw.Elapsed;
		var elapsedForAddOperations = swForAddOperations.Elapsed;

		for (int n = 0; n < 2; n++)
		{
			sw = Stopwatch.StartNew(); var list = new List<NonEmptyGuid>(nbIterationsIncludingInitIteration); list.Add(someData);
			swForAddOperations = Stopwatch.StartNew(); for (int i = 0; i < nbIterations; i++) { list.Add(data[i]); }
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "List<int>.Add";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var listBuilder = GenericListBuilder.For(dataType); listBuilder.Add(someData);
			swForAddOperations.Restart(); for (int i = 0; i < nbIterations; i++) { listBuilder.Add(data[i]); }
			var listBuiltUsingAdd = listBuilder.BuildAsIList();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericListBuilder.Add";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var listBuiltUsingAddRangeBuilder = GenericListBuilder.For(dataType); listBuiltUsingAddRangeBuilder.Add(someData);
			swForAddOperations.Restart(); listBuiltUsingAddRangeBuilder.AddRange(data).BuildAsIList();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericListBuilder.AddRange";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var listBuiltUsingAddRangeBuilderWithKnowCapacity = GenericListBuilder.For(dataType, nbIterationsIncludingInitIteration); listBuiltUsingAddRangeBuilderWithKnowCapacity.Add(someData);
			swForAddOperations.Restart(); listBuiltUsingAddRangeBuilderWithKnowCapacity.AddRange(data).BuildAsIList();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericListBuilder.AddRange+capacity";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}
	}

	class Log
	{
		readonly string _testId;
		readonly Type _itemType;
		readonly int _nbIterations;
		public string BenchmarkCase { get; set; } = "";
		Log(string testId, Type itemType, int nbIterations)
		{
			_testId = testId;
			_itemType = itemType;
			_nbIterations = nbIterations;
			Console.WriteLine($"{_testId} - List<{_itemType.GetName()}>/{_nbIterations:N0} elements");
		}
		public static Log ForCurrentTest(Type itemType, int nbIterations, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMethodName = "")
		{
			var testId = $"{Path.GetFileNameWithoutExtension(callerFilePath)}.{callerMethodName}";
			return new(testId, itemType, nbIterations);
		}

		public void CreationTime(TimeSpan elapsed)
		=> Console.WriteLine($"  {$"{{{BenchmarkCase}}}",-40}{"FullBuild",-15} {elapsed.TotalMilliseconds,5:N2} ms (mean:{elapsed.TotalNanoseconds / _nbIterations+1,3:N0} ns)");

		public void AddOperationsTime(TimeSpan elapsedForAddOperations)
		=> Console.WriteLine($"  {$"{{{BenchmarkCase}}}",-40}{"> AddOperations",-15} {elapsedForAddOperations.TotalMilliseconds,5:N2} ms (mean:{elapsedForAddOperations.TotalNanoseconds / _nbIterations,3:N0} ns)");
	}
}
