using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.PerformanceBenchmarks;
using System.Diagnostics;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Performances;

[PerformanceBenchmark]
class ConstrainedTypeInstanciationBenchmarks
{
	[Test]
	public void NonEmptyGuid_Instanciation()
	{
		var nbIterations = 1000;
		var nbIterationsAfterInitialCall = nbIterations + 1;
		var v = ConstrainedTypeInfos.TryGet(typeof(NonEmptyGuid), out var constrainedTypeInfo);
		var guids = new Guid[nbIterationsAfterInitialCall];
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			guids[i] = Guid.NewGuid();
		}

		var sw = Stopwatch.StartNew();
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = NonEmptyGuid.ApplyConstraintsTo(guids[i]);
		}
		var elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} NonEmptyGuid.ApplyConstraintsTo (direct static method call)");

		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = constrainedTypeInfo.InvokeStaticFactoryMethod(guids[i]);
		}
		elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} MethodInfoFoundByInterface.Invoke (reflection on interface method)");
	}

	[Test]
	public void NonEmptyGuidStartingWithTheCharacter3_Instanciation()
	{
		var nbIterations = 1000;
		var nbIterationsAfterInitialCall = nbIterations + 1;
		var v = ConstrainedTypeInfos.TryGet(typeof(ClassArchetype.NonEmptyGuidStartingWithTheCharacter3), out var constrainedTypeInfo);
		var guids = new Guid[nbIterationsAfterInitialCall];
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			var guid = Guid.NewGuid();
			var guid3 = Guid.Parse($"3{guid.ToString()[1..]}");
			guids[i] = guid3;
		}

		var sw = Stopwatch.StartNew();
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo(guids[i]);
		}
		var elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} {nameof(ClassArchetype.NonEmptyGuidStartingWithTheCharacter3)}.ApplyConstraintsTo (direct static method call)");

		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = constrainedTypeInfo.InvokeStaticFactoryMethod(guids[i]);
		}
		elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} MethodInfoFoundByInterface.Invoke (reflection on interface method)");
	}

	[Test]
	public void NonEmptyHashSet_Instanciation()
	{
		var nbIterations = 1000;
		var nbIterationsAfterInitialCall = nbIterations + 1;
		var v = ConstrainedTypeInfos.TryGet(typeof(NonEmptyHashSet<>), out var constrainedTypeInfo);
		var arrayOfGuidArrays = new List<Guid[]>(nbIterationsAfterInitialCall);
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			arrayOfGuidArrays.Add(new Guid[] { Guid.NewGuid() });
		}

		var sw = Stopwatch.StartNew();
		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = NonEmptyHashSet.CreateFromElements(arrayOfGuidArrays[i]);
		}
		var elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} NonEmptyHashSet.ApplyConstraintsTo (direct static method call)");

		for (var i = 0; i < nbIterationsAfterInitialCall; i++)
		{
			if (i == 1) { sw.Restart(); }
			var x = constrainedTypeInfo.InvokeStaticFactoryMethod(arrayOfGuidArrays[i]);
		}
		elapsed = sw.Elapsed;
		Console.WriteLine($"{elapsed.TotalMilliseconds,10:N2} ms (mean:{elapsed.TotalNanoseconds / nbIterations,3:N0} ns) spent calling {nbIterations:N0} MethodInfoFoundByInterface.Invoke (reflection on interface method)");
	}
}
