namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

class InterfaceImplementatingGeneratorShould
{
	[Test, Repeat(2)]
	public void BeAbleTo_Generate_A_Dummy_Instance()
	{
		// Arrange
		var dummy = ClassArchetype.SomeRepositoryTestDouble.Dummy;

		// Act & Assert
		using (Assert.EnterMultipleScope())
		{
			Assert.That(
				dummy.Write(Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingValuePropertyType>()),
				Is.EqualTo(Some.Valid<int>()));

			Assert.That(
				dummy.Read(Randomized.Valid<int>()),
				Is.EqualTo(Some.Valid<ClassArchetype.PositionalRecordTypeContainingValuePropertyType>()));

			Assert.That(
				dummy.ReturnStrictlyPositiveInt(),
				Is.EqualTo(Some.Valid<StrictlyPositiveInt>()));

			void callDoSomething() => dummy.DoSomething(Randomized.Valid<int>());
			Assert.That(callDoSomething, Throws.Nothing);
		}
	}
public record SomeGenericRepositoryTestDouble<T>
{
}
	[Test, Repeat(2)]
	public void BeAbleTo_Generate_A_Dummy_InstanceOfClosedGenericInterface()
	{
		// Arrange
		var dummy = ClassArchetype.SomeGenericRepositoryTestDouble<string>.Dummy;

		// Act & Assert
		using (Assert.EnterMultipleScope())
		{
			Assert.That(
				dummy.Write(Randomized.Valid<string>()),
				Is.EqualTo(Some.Valid<int>()));

			Assert.That(
				dummy.Read(Randomized.Valid<int>()),
				Is.EqualTo(Some.Valid<string>()));

			Assert.That(
				dummy.ReturnStrictlyPositiveInt(),
				Is.EqualTo(Some.Valid<StrictlyPositiveInt>()));

			void callDoSomething() => dummy.DoSomething(Randomized.Valid<int>());
			Assert.That(callDoSomething, Throws.Nothing);
		}
	}
}
