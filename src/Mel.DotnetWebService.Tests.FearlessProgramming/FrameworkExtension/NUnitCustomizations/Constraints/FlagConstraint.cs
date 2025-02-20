using System.Reflection;
using NUnit.Framework.Constraints;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.Constraints;

public class FlagConstraint : Constraint
{
	readonly object _expectedFlag;
	public FlagConstraint(object expected) : base(expected)
	{
		_expectedFlag = expected;
	}

	public override string Description => $"enum flag {_expectedFlag}";

	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		var actualValue = Convert.ToUInt64(actual);
		var flagValue = Convert.ToUInt64(_expectedFlag);

		var success = (actualValue & flagValue) == flagValue;

		return new ConstraintResult(this, actual, success);
	}
}
