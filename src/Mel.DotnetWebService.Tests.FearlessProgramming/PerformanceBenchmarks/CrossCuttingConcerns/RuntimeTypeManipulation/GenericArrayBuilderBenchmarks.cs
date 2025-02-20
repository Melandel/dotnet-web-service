using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.PerformanceBenchmarks.CrossCuttingConcerns.RuntimeTypeManipulation;

[PerformanceBenchmark]
class GenericArrayBuilderBenchmarks
{
	[Test]
	public void Array_of_int()
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
			sw.Restart(); var array = new int[nbIterationsIncludingInitIteration]; array[0] = someData;
			swForAddOperations.Restart(); for (int i = 0; i < nbIterations; i++) { array[i] = data[i]; }
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "int[].set";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var array2 = new int[nbIterationsIncludingInitIteration]; array2[0] = someData;
			swForAddOperations.Restart(); data.CopyTo(array2, 0);
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "Array.CopyTo";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var arrayBuilder = GenericArrayBuilder.ForACapacityOf(nbIterationsIncludingInitIteration, dataType); arrayBuilder.Add(someData);
			swForAddOperations.Restart(); for (int i = 0; i < nbIterations; i++) { arrayBuilder.Add(data[i]); }
			var arrayBuiltUsingAdd = arrayBuilder.BuildAsArray();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericArrayBuilder.Add";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var arrayBuiltUsingAddRangeBuilder = GenericArrayBuilder.ForACapacityOf(nbIterationsIncludingInitIteration, dataType); arrayBuiltUsingAddRangeBuilder.Add(someData);
			swForAddOperations.Restart(); arrayBuiltUsingAddRangeBuilder.AddRange(data).BuildAsIList();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericArrayBuilder.AddRange";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}
	}

	[Test]
	public void Array_of_NonEmptyGuids()
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
			sw = Stopwatch.StartNew(); var array = new NonEmptyGuid[nbIterationsIncludingInitIteration]; array[0] = someData;
			swForAddOperations = Stopwatch.StartNew(); for (int i = 0; i < nbIterations; i++) { array[i] = data[i]; }
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "int[]:set";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var arrayBuilder = GenericArrayBuilder.ForACapacityOf(nbIterationsIncludingInitIteration, dataType); arrayBuilder.Add(someData);
			swForAddOperations.Restart(); for (int i = 0; i < nbIterations; i++) { arrayBuilder.Add(data[i]); }
			var arrayBuiltUsingAdd = arrayBuilder.BuildAsArray();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericArrayBuilder.Add";
			log.CreationTime(elapsed);
			log.AddOperationsTime(elapsedForAddOperations);
		}

		for (int n = 0; n < 2; n++)
		{
			sw.Restart(); var arrayBuiltUsingAddRangeBuilder = GenericArrayBuilder.ForACapacityOf(nbIterationsIncludingInitIteration, dataType); arrayBuiltUsingAddRangeBuilder.Add(someData);
			swForAddOperations.Restart(); arrayBuiltUsingAddRangeBuilder.AddRange(data).BuildAsIList();
			elapsed = sw.Elapsed;
			elapsedForAddOperations = swForAddOperations.Elapsed;

			if (n == 0) { continue; }
			log.BenchmarkCase = "GenericArrayBuilder.AddRange";
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
			Console.WriteLine($"{_testId} - {_itemType.GetName()}[] with {_nbIterations:N0} elements");
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
