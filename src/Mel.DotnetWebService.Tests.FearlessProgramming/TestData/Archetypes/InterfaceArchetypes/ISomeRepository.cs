namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static partial class InterfaceArchetype
{
	public interface ISomeRepositoryInterface
	{
		int Write(ClassArchetype.PositionalRecordTypeContainingValuePropertyType record);
		ClassArchetype.PositionalRecordTypeContainingValuePropertyType Read(int index);
		void  DoSomething(int index);
		StrictlyPositiveInt ReturnStrictlyPositiveInt();
	}
}
