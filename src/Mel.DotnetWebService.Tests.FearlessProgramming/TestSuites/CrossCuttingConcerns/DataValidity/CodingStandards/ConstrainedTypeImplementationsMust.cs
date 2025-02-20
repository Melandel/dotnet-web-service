namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.CodingStandards;

class ConstrainedTypeImplementationsMust
{
	[TestCaseSource(typeof(Types), nameof(Types.AllConcreteConstrainedTypes))]
	public void Simultaneously_InheritConstrainedBaseClass_And_ImplementIConstrainedInterface(Type typeInheritingTheConstrainedBaseClass)
	{
		Assert.That(typeInheritingTheConstrainedBaseClass.ImplementsInterface(typeof(IConstrainedValue<,>)), Is.True);
	}
}
