using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

class ConstrainedTypeProviderShould
{
	[TestCaseSource(typeof(Types), nameof(Types.AllConcreteConstrainedTypes))]
	public void Foo(Type type)
	{
		var isDetectedAtRuntime = ConstrainedTypeInfos.TryGet(type, out var constrainedType);

		Assert.That(isDetectedAtRuntime, Is.True);

		foreach (var validValue in constrainedType.ValidValueExamples)
		{
			Assert.That(
				() => constrainedType.InvokeStaticFactoryMethod(validValue),
				Throws.Nothing);
		}

		foreach ((var invalidValue, var errorMessage) in constrainedType.ErrorMessagesByInvalidValueExample)
		{
			Assert.That(
				() => constrainedType.InvokeStaticFactoryMethod(invalidValue),
				Throws.Exception
					.AssignableTo<ObjectConstructionException>()
					.With.Message.Contains(errorMessage));
		}
	}

	[Test]
	public void Bar()
	{
		var guid = Guid.Parse("dbca02a1-1288-496d-8095-fe1dde12af8b");
		Assert.That(NonEmptyGuid.ApplyConstraintsTo(guid) == guid, Is.True);
	}
}
