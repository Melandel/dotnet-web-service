using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.NUnitCustomizations.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class RetryAttribute : NUnit.Framework.RetryAttribute, IApplyToTest
{
	public RetryAttribute(int count) : base(count)
	{
	}

	public void ApplyToTest(Test test)
	{
		test.Properties.Set(nameof(RetryAttribute), true);
	}
}

