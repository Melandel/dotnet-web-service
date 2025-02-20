using NUnit.Framework.Constraints;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.Constraints;

public class EqualConstraint : NUnit.Framework.Constraints.EqualConstraint
{
	public EqualConstraint(object expected) : base(expected)
	{
	}

	public override string Description => base.Description;

	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		var nativeConstraintResult = base.ApplyTo(actual);
		if (nativeConstraintResult.Status == ConstraintStatus.Failure)
		{
			if (actual.HasUserDefinedConversions(out var converters) && converters.Any(c => Arguments[0].Equals(c.Invoke(null, new object[] { actual }))))
			{
				return new ConstraintResult(this, actual, true);
			}
		}

		return nativeConstraintResult;
	}
}
