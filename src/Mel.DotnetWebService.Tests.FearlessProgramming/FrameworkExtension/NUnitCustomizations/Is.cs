namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations;

class Is : NUnit.Framework.Is
{
	public new static NUnit.Framework.Constraints.EqualConstraint EqualTo(object expected)
	{
		return new Constraints.EqualConstraint(expected);
	}
}
