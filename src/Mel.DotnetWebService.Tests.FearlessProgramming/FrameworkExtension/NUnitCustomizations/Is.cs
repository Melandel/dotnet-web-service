using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations;

class Is : NUnit.Framework.Is
{
	public new static NUnit.Framework.Constraints.EqualConstraint EqualTo(object expected)
	{
		return new Constraints.EqualConstraint(expected);
	}

	public static NUnit.Framework.Constraints.CollectionEquivalentConstraint EquivalentTo(object expected)
	=> expected switch
	{
		IEnumerable ienumerableExpected => EquivalentTo(ienumerableExpected),
		IConstrainedType iconstrainedTypeExpected => EquivalentTo(iconstrainedTypeExpected),
		_ => throw new InvalidOperationException($"Constraint {nameof(Is)}.{nameof(EquivalentTo)} does not support type {expected.GetType().GetName()} (value: {expected.GetStringRepresentation()})")
	};

	public static new NUnit.Framework.Constraints.CollectionEquivalentConstraint EquivalentTo(IEnumerable expected)
	=> new Constraints.CollectionEquivalentConstraint(expected);

	public static NUnit.Framework.Constraints.CollectionEquivalentConstraint EquivalentTo(IConstrainedType expected)
	{
		if (!ConstrainedTypeInfos.TryGet(expected.GetType(), out var constrainedTypeInfo))
		{
			throw new InvalidOperationException($"Constraint {nameof(Is)}.{nameof(EquivalentTo)} does not support type {expected.GetType().GetName()} (value: {expected.GetStringRepresentation()})");
		}
		IEnumerable comparableExpected = constrainedTypeInfo.InvokeImplicitConversionToRootType(expected);
		return EquivalentTo(comparableExpected);
	}
}
