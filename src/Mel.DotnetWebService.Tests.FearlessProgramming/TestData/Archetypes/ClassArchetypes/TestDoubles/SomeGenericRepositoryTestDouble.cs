namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class ClassArchetype
{
	public record SomeGenericRepositoryTestDouble<T> : TestDouble<InterfaceArchetype.ISomeGenericRepositoryInterface<T>, SomeGenericRepositoryTestDouble<T>>, InterfaceArchetype.ISomeGenericRepositoryInterface<T>
	{
		public void DoSomething(int index) => throw new NotImplementedException();
		public T Read(int index) => throw new NotImplementedException();
		public StrictlyPositiveInt ReturnStrictlyPositiveInt() => throw new NotImplementedException();
		public int Write(T record) => throw new NotImplementedException();
	}
}
