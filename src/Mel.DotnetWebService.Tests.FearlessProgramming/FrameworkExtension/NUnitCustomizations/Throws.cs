using Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ExceptionThrowingStubGeneration;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations;

class Throws : NUnit.Framework.Throws
{
	public static NUnit.Framework.Constraints.AssignableToConstraint ExceptionSpecificToStubs
	=> NUnit.Framework.Throws.Exception.AssignableTo(ExceptionUnknownAtCompileTime.Type);
}
