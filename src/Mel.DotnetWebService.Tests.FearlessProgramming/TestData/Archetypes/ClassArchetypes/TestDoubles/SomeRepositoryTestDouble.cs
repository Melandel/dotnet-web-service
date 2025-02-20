namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public record SomeRepositoryTestDouble : TestDouble<InterfaceArchetype.ISomeRepositoryInterface, SomeRepositoryTestDouble>, InterfaceArchetype.ISomeRepositoryInterface
	{
		public void DoSomething(int index) => throw new NotImplementedException();
		public PositionalRecordTypeContainingValuePropertyType Read(int index) => throw new NotImplementedException();
		public StrictlyPositiveInt ReturnStrictlyPositiveInt() => throw new NotImplementedException();
		public int Write(PositionalRecordTypeContainingValuePropertyType record) => throw new NotImplementedException();
	}
}
