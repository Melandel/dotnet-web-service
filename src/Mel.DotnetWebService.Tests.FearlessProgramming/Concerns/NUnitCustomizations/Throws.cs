using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.DynamicallyGeneratedTypes;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.NUnitCustomizations;

class Throws : NUnit.Framework.Throws
{
	public static NUnit.Framework.Constraints.AssignableToConstraint ExceptionSpecificToStubs
	=> NUnit.Framework.Throws.Exception.AssignableTo(ExceptionUnknownAtCompileTime.Type);
}
