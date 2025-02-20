using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

class ConstrainedTypeProviderShould
{

	[TestCaseSource(typeof(Types), nameof(Types.AllConcreteConstrainedTypes))]
	public void ProvideConcreteExampleValuesBothValidAndInvalid(Type type)
	{
		var isDetectedAtRuntime = ConstrainedTypeInfos.TryGet(type, out var constrainedType);

		Assert.That(isDetectedAtRuntime, Is.True);

		foreach (var validValue in constrainedType.ValidValueExamples)
		{
			Action action = () => constrainedType.InvokeStaticFactoryMethod(validValue);
			Assert.DoesNotThrow(action);
		}

		foreach (var constraintViolationExample in constrainedType.ErrorMessagesByInvalidNativeRootTypeValueExample)
		{
			Action action = () => constrainedType.InvokeStaticFactoryMethod(constraintViolationExample.ExampleValue);
			Assert.That(
				action,
				Throws.Exception.AssignableTo<ObjectConstructionException>()
					.With.Message.Contain(constraintViolationExample.ErrorMessage));
		}
	}

	//[TestCaseSource(typeof(Types), nameof(Types.AllConcreteConstrainedTypes))]
	//public void ProvideConcreteExampleValuesBothValidAndInvalid2(Type type)
	//{
	//	var provider = DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime.ConstrainedTypeInfoProvider.LoadConstrainedTypesDeclaredIn(Assembly.GetExecutingAssembly());
	//	var isDetectedAtRuntime = provider.TryGet(type, out var constrainedType);

	//	Assert.That(isDetectedAtRuntime, Is.True);

	//	foreach (var validValue in constrainedType.ValidValueExamples)
	//	{
	//		Assert.That(
	//			() => constrainedType.InvokeStaticFactoryMethod(validValue),
	//			Throws.Nothing);
	//	}

	//	foreach (var constraintViolationExample in constrainedType.ErrorMessagesByInvalidNativeRootTypeValueExample)
	//	{
	//		Assert.That(
	//			() => constrainedType.InvokeStaticFactoryMethod(constraintViolationExample.ExampleValue),
	//			Throws.Exception
	//				.AssignableTo<ObjectConstructionException>()
	//				.With.Message.Contains(constraintViolationExample.ErrorMessage));
	//	}

	//	if (type == typeof(NonEmptyString))
	//	{
	//		Assert.That(
	//			() => NonEmptyString.ApplyConstraintsTo(null),
	//			Throws.Exception.AssignableTo<ObjectConstructionException>().With.Message.Contains("Value must exist"));
	//	}
	//}
}
