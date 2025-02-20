using System.Collections;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations;

class Is : NUnit.Framework.Is
{
	public new static NUnit.Framework.Constraints.EqualConstraint EqualTo(object expected)
	{
		return new Constraints.EqualConstraint(expected);
	}

	public new static NUnit.Framework.Constraints.CollectionEquivalentConstraint EquivalentTo(IEnumerable expected)
	{
		return new Constraints.CollectionEquivalentConstraint(expected);
	}
}
