namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class InterfaceArchetype
{
	public interface ISomeGenericRepositoryInterface<T>
	{
		int Write(T record);
		T Read(int index);
		void  DoSomething(int index);
		StrictlyPositiveInt ReturnStrictlyPositiveInt();
	}
}
