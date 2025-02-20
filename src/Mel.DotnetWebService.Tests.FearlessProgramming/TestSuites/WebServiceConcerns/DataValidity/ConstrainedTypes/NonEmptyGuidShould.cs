using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.DataValidity.ConstrainedTypes;

class NonEmptyGuidShould
{
	[Test]
	public void Be_Consistent_When_Encapsulating_A_NonEmpty_Guid()
	{
		// Arrange
		var nonEmptyGuid = Guid.NewGuid();

		// Act
		var instance = NonEmptyGuid.FromGuid(nonEmptyGuid);

		// Assert
#pragma warning disable NUnit2021 // Incompatible types for EqualTo constraint
		Assert.That(instance, Is.EqualTo(nonEmptyGuid));
#pragma warning restore NUnit2021 // Incompatible types for EqualTo constraint
	}

	[Test]
	public void Throw_When_Constructed_From_Empty_Guid()
	=> Assert.That(
		() => { var instance = NonEmptyGuid.FromGuid(Guid.Empty); },
		Throws.Exception
			.AssignableTo<ObjectConstructionException>()
			.With.Message.EqualTo(
				string.Join(Environment.NewLine,
					"NonEmptyGuid: construction failed via NonEmptyGuid.FromGuid(Guid) with the following parameter: \"00000000-0000-0000-0000-000000000000\" [Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids]",
					"NonEmptyGuid: Value cannot accept value \"00000000-0000-0000-0000-000000000000\": Value must not be empty [Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids]")));
}
