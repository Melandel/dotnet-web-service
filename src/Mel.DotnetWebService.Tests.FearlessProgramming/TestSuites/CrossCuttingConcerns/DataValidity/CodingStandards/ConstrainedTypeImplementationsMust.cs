namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.CodingStandards;

class ConstrainedTypeImplementationsMust
{
	[TestCaseSource(typeof(Types), nameof(Types.AllConcreteConstrainedTypes))]
	public void Simultaneously_InheritConstrainedBaseClass_And_ImplementIConstrainedInterface(Type constrainedType)
	{
		if (constrainedType.IsOrExtends(typeof(ConstrainedFurthermore<>)))
		{
			Assert.That(
				constrainedType.ImplementsInterface(typeof(IConstrainedValue<,>))
				|| constrainedType.ImplementsInterface(typeof(IConstrainedCollection<,>))
				|| constrainedType.ImplementsInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>)));
			return;
		}

		if (constrainedType.ImplementsInterface(typeof(IConstrainedCollection<,>)))
		{
			Assert.That(constrainedType.IsOrExtends(typeof(ConstrainedCollection<>)));
			return;
		}

		if (constrainedType.ImplementsInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>)))
		{
			Assert.That(constrainedType.IsOrExtends(typeof(ConstrainedCollectionOfKeyValuePairs<>)));
			return;
		}

		if (constrainedType.ImplementsInterface(typeof(IConstrainedValue<,>)))
		{
			Assert.That(constrainedType.IsOrExtends(typeof(ConstrainedValue<>)));
			return;
		}

		Assert.Fail($"{constrainedType} is not a valid constrained type");
	}
}
