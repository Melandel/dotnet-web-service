using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.PerformanceBenchmarks;
using System.Diagnostics;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Performances;

[PerformanceBenchmark]
class ConstrainedTypeConversionBenchmarks
{
	[Test]
	public void NonEmptyGuid_Conversion()
	{
		var nbIterations = 1000;
		var nbIterationsAfterInitialCall = nbIterations + 1;
		var v = ConstrainedTypeInfos.TryGet(typeof(NonEmptyGuid), out var constrainedTypeInfo);
		var nonEmptyGuids = new NonEmptyGuid[nbIterationsAfterInitialCall];
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			nonEmptyGuids[i] = NonEmptyGuid.ApplyConstraintsTo(Guid.NewGuid());
		}

		var sw = Stopwatch.StartNew();
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = (Guid)nonEmptyGuids[i];
		}
		var elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} NonEmptyGuid.ImplicitConversionToGuid (direct static method call)");

		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = constrainedTypeInfo.InvokeImplicitConversionToRootType(nonEmptyGuids[i]);
		}
		elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} MethodInfo.Compile.Invoke (reflection on interface method)");
	}

	[Test]
	public void NonEmptyGuidStartingWithTheCharacter3_Conversion()
	{
		var nbIterations = 1000;
		var nbIterationsAfterInitialCall = nbIterations + 1;
		var v = ConstrainedTypeInfos.TryGet(typeof(ClassArchetype.NonEmptyGuidStartingWithTheCharacter3), out var constrainedTypeInfo);
		var nonEmptyGuids3 = new ClassArchetype.NonEmptyGuidStartingWithTheCharacter3[nbIterationsAfterInitialCall];
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			var guid = Guid.NewGuid();
			var guid3 = Guid.Parse($"3{guid.ToString()[1..]}");
			nonEmptyGuids3[i] = ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo(guid3);
		}

		var sw = Stopwatch.StartNew();
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = (Guid)(NonEmptyGuid)(nonEmptyGuids3[i]);
		}
		var elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} {nameof(ClassArchetype.NonEmptyGuidStartingWithTheCharacter3)}.ImplicitConversionToGuid (direct static method call)");

		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = constrainedTypeInfo.InvokeImplicitConversionToRootType(nonEmptyGuids3[i]);
		}
		elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} MethodInfo.Compile.Invoke (reflection on interface method)");
	}

	[Test]
	public void NonEmptyHashSet_Conversion()
	{
		var nbIterations = 1000;
		var nbIterationsAfterInitialCall = nbIterations + 1;
		var v = ConstrainedTypeInfos.TryGet(typeof(NonEmptyHashSet<>), out var constrainedTypeInfo);
		var arrayOfNonEmptyHashSets = new List<NonEmptyHashSet<Guid>>(nbIterationsAfterInitialCall);
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			arrayOfNonEmptyHashSets.Add(NonEmptyHashSet.ApplyConstraintsTo(new Guid[] { Guid.NewGuid() }));
		}

		var sw = Stopwatch.StartNew();
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = (HashSet<Guid>)arrayOfNonEmptyHashSets[i];
		}
		var elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} NonEmptyHashSet.ImplicitConversionToGuid (direct static method call)");

		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = constrainedTypeInfo.InvokeImplicitConversionToRootType(arrayOfNonEmptyHashSets[i]);
		}
		elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} MethodInfo.Compile.Invoke (reflection on interface method)");
	}
}
