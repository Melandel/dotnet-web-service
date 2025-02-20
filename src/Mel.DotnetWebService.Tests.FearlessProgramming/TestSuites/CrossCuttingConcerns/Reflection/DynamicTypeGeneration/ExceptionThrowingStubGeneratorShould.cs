namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

class ExceptionThrowingStubGeneratorShould
{
	[Test, Repeat(2)]
	public void BeAbleTo_Generate_An_Instance_That_Always_Throws()
	{
		// Arrange
		var stub = ClassArchetype.SomeRepositoryTestDouble.StubThatThrows;

		// Act & Assert
		using (Assert.EnterMultipleScope())
		{
			int writeSomething() => stub.Write(Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingValuePropertyType>());
			Assert.That(
				writeSomething,
				Throws.ExceptionSpecificToStubs);

			ClassArchetype.PositionalRecordTypeContainingValuePropertyType readSomething() => stub.Read(Randomized.Valid<int>());
			Assert.That(
				readSomething,
				Throws.ExceptionSpecificToStubs);

			StrictlyPositiveInt returnStrictlyPositiveInt() => stub.ReturnStrictlyPositiveInt();
			Assert.That(
				returnStrictlyPositiveInt,
				Throws.ExceptionSpecificToStubs);

			void doSomething() => stub.DoSomething(Randomized.Valid<int>());
			Assert.That(
				doSomething,
				Throws.ExceptionSpecificToStubs);
		}
	}

	[Test, Repeat(2)]
	public void BeAbleTo_Generate_An_InstanceOfClosedGenericInterface_That_Always_Throws()
	{
		// Arrange
		var stub = ClassArchetype.SomeGenericRepositoryTestDouble<string>.StubThatThrows;

		// Act & Assert
		using (Assert.EnterMultipleScope())
		{
			int writeSomething() => stub.Write(Randomized.Valid<string>());
			Assert.That(
				writeSomething,
				Throws.ExceptionSpecificToStubs);

			string readSomething() => stub.Read(Randomized.Valid<int>());
			Assert.That(
				readSomething,
				Throws.ExceptionSpecificToStubs);

			StrictlyPositiveInt returnStrictlyPositiveInt() => stub.ReturnStrictlyPositiveInt();
			Assert.That(
				returnStrictlyPositiveInt,
				Throws.ExceptionSpecificToStubs);

			void doSomething() => stub.DoSomething(Randomized.Valid<int>());
			Assert.That(
				doSomething,
				Throws.ExceptionSpecificToStubs);
		}
	}
}
