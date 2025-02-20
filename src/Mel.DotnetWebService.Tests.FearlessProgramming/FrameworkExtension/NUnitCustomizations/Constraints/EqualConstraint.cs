using System.Reflection;
using NUnit.Framework.Constraints;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.Constraints;

public class EqualConstraint : NUnit.Framework.Constraints.EqualConstraint
{
	public EqualConstraint(object expected) : base(expected)
	{
	}

	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		var nativeConstraintResult = base.ApplyTo(actual);
		if (nativeConstraintResult.Status == ConstraintStatus.Failure)
		{
			if (actual.HasUserDefinedConversions(out var converters) && FindEqualityUsingAnyConverter(actual, converters))
			{
				return new ConstraintResult(this, actual, true);
			}
		}

		return nativeConstraintResult;
	}

	bool FindEqualityUsingAnyConverter(object actual, MethodInfo[] converters)
	{
		var expected = Arguments[0];
		foreach (var converter in converters)
		{
			try
			{
				var converted = converter.Invoke(null, new[] { actual });
				if (expected.Equals(converted))
				{
					return true;
				}
			}
			catch
			{
			}
		}

		return false;
	}
}
