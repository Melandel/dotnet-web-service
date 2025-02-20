namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations;

class Has : NUnit.Framework.Has
{
	public static NUnit.Framework.Constraints.Constraint Flag(object expected)
	{
		return new Constraints.FlagConstraint(expected);
	}
}
