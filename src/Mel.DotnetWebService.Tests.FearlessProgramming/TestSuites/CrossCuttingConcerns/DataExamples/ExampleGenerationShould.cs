using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataExamples;

class ExampleGenerationShould
{
	[Test]
	public void BeAbleTo_Instantiate_Examples_For_DeserializationAdverseTypes()
	=> ExampleValueGeneratorTestCases.DeserializationAdverseLeafObjectsAggregatedIntoSingleTestCase
		.ParallelForEach((testCase, failingTestMessage) =>
		{
			// Act
			var generatedExample = ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 1);

			// Assert
			var exampleStringRepresentation = generatedExample.GetStringRepresentation();

			Assert.That(
				exampleStringRepresentation,
				Is.EqualTo(testCase.ExpectedStringRepresentationForSalt1),
				string.Join(Environment.NewLine,
				[
					$"[EXAMPLE VALUE GENERATION] {failingTestMessage}",
				$"Expected {testCase.ExpectedStringRepresentationForSalt1}",
				$"Got      {exampleStringRepresentation}"
				]));
		});

	[Test]
	public void Instantiate_Different_Examples_For_Different_Salt_Values()
	=> ExampleValueGeneratorTestCases.AllTestCasesExceptThoseInvolvingSingletonsAggregatedIntoOne
		.ParallelForEach((testCase, failingTestMessage) =>
		{
			var salt0 = ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 0).GetStringRepresentation();
			var salt1 = testCase.ExpectedStringRepresentationForSalt1;
			var salt2 = ExampleValueGenerator.GenerateExampleOf(testCase.Type, salt: 2).GetStringRepresentation();

			using (Assert.EnterMultipleScope())
			{
				Assert.That(salt0, Is.Not.EqualTo(salt1), failingTestMessage);
				Assert.That(salt0, Is.Not.EqualTo(salt2), failingTestMessage);
				Assert.That(salt1, Is.Not.EqualTo(salt2), failingTestMessage);
			}
		});

	[Test]
	public void BeAbleTo_Instantiate_ParameterlessFuncTypes()
	{
		var parameterlessFunc0 = ExampleValueGenerator.GenerateExampleOf<Func<int>>();
		Assert.That(parameterlessFunc0.Invoke(), Is.EqualTo(ExampleValueGenerator.GenerateExampleOf<int>()));

		var parameterlessFunc1 = ExampleValueGenerator.GenerateExampleOf<Func<int>>(1);
		Assert.That(parameterlessFunc1.Invoke(), Is.EqualTo(ExampleValueGenerator.GenerateExampleOf<int>(1)));
	}

	[Test]
	public void BeAbleTo_Instantiate_FuncTypesWithMultipleParameters()
	{
		var parameterlessFunc0 = ExampleValueGenerator.GenerateExampleOf<Func<int, object, string>>();
		Assert.That(parameterlessFunc0.Invoke(0, null), Is.EqualTo(ExampleValueGenerator.GenerateExampleOf<string>()));

		var parameterlessFunc1 = ExampleValueGenerator.GenerateExampleOf<Func<int, object, string>>(1);
		Assert.That(parameterlessFunc1.Invoke(1, new object()), Is.EqualTo(ExampleValueGenerator.GenerateExampleOf<string>(1)));
	}

	[Test]
	public void BeAbleTo_Instantiate_ActionTypes()
	{
		var parameterlessAction = ExampleValueGenerator.GenerateExampleOf<Action>();
		Action action = () => parameterlessAction.Invoke();
		Assert.That(action, Throws.Nothing);

		var actionWithParameters = ExampleValueGenerator.GenerateExampleOf<Action<string>>();
		Action actionWithParams = () => actionWithParameters.Invoke(Some.Valid<string>());
		Assert.That(actionWithParams, Throws.Nothing);
	}

	[Test]
	public void BeAbleTo_Instantiate_NonGenericTypesWithOnlyGenericPublicInstantiationOperations()
	{
		var salt0 = ExampleValueGenerator.GenerateExampleOf<ObjectConstructionException>().GetStringRepresentation();
		var salt1 = ExampleValueGenerator.GenerateExampleOf<ObjectConstructionException>(1).GetStringRepresentation();
		var salt2 = ExampleValueGenerator.GenerateExampleOf<ObjectConstructionException>(2).GetStringRepresentation();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(salt0, Is.Not.EqualTo(salt1));
			Assert.That(salt1, Is.Not.EqualTo(salt2));
			Assert.That(salt2, Is.Not.EqualTo(salt0));
		}
	}

	[Test]
	public void BeAbleTo_Instantiate_The_Type_Lazy()
	{
		var salt0 = ExampleValueGenerator.GenerateExampleOf<Lazy<int>>().GetStringRepresentation();
		var salt1 = ExampleValueGenerator.GenerateExampleOf<Lazy<int>>(1).GetStringRepresentation();
		var salt2 = ExampleValueGenerator.GenerateExampleOf<Lazy<int>>(2).GetStringRepresentation();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(salt0, Is.Not.EqualTo(salt1));
			Assert.That(salt1, Is.Not.EqualTo(salt2));
			Assert.That(salt2, Is.Not.EqualTo(salt0));
		}
	}

	[Test]
	public void BeAbleTo_Instantiate_The_Type_Type()
	{
		var salt0 = ExampleValueGenerator.GenerateExampleOf<Type>().GetStringRepresentation();
		var salt1 = ExampleValueGenerator.GenerateExampleOf<Type>(1).GetStringRepresentation();
		var salt2 = ExampleValueGenerator.GenerateExampleOf<Type>(2).GetStringRepresentation();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(salt0, Is.Not.EqualTo(salt1));
			Assert.That(salt1, Is.Not.EqualTo(salt2));
			Assert.That(salt2, Is.Not.EqualTo(salt0));
		}
	}

	[Test]
	public void BeAbleTo_Instantiate_RecursiveTypes()
	{
		var salt0 = ExampleValueGenerator.GenerateExampleOf<Exception>().GetStringRepresentation();
		var salt1 = ExampleValueGenerator.GenerateExampleOf<Exception>(1).GetStringRepresentation();
		var salt2 = ExampleValueGenerator.GenerateExampleOf<Exception>(2).GetStringRepresentation();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(salt0, Is.Not.EqualTo(salt1));
			Assert.That(salt1, Is.Not.EqualTo(salt2));
			Assert.That(salt2, Is.Not.EqualTo(salt0));
		}
	}

	[Test]
	public void BeAbleTo_Instantiate_The_Type_IOptions()
	{
		var salt0 = ExampleValueGenerator.GenerateExampleOf<IOptions<NonEmptyString>>().GetStringRepresentation();
		var salt1 = ExampleValueGenerator.GenerateExampleOf<IOptions<NonEmptyString>>(1).GetStringRepresentation();
		var salt2 = ExampleValueGenerator.GenerateExampleOf<IOptions<NonEmptyString>>(2).GetStringRepresentation();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(salt0, Is.Not.EqualTo(salt1));
			Assert.That(salt1, Is.Not.EqualTo(salt2));
			Assert.That(salt2, Is.Not.EqualTo(salt0));
		}
	}

	[Test]
	public void BeAbleTo_Instantiate_The_Type_IOptionsSnapshot()
	{
		var salt0 = ExampleValueGenerator.GenerateExampleOf<IOptionsSnapshot<StrictlyPositiveInt>>().GetStringRepresentation();
		var salt1 = ExampleValueGenerator.GenerateExampleOf<IOptionsSnapshot<StrictlyPositiveInt>>(1).GetStringRepresentation();
		var salt2 = ExampleValueGenerator.GenerateExampleOf<IOptionsSnapshot<StrictlyPositiveInt>>(2).GetStringRepresentation();

		using (Assert.EnterMultipleScope())
		{
			Assert.That(salt0, Is.Not.EqualTo(salt1));
			Assert.That(salt1, Is.Not.EqualTo(salt2));
			Assert.That(salt2, Is.Not.EqualTo(salt0));
		}
	}
}
