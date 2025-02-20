using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

class ConfigurationShould
{
	[Test]
	public void BeAbleTo_Bind_Directly_Into_ConstrainedType()
	{
		// Arrange
		var stringSetting = Some.Valid<string>();
		var nonEmptyGuidSetting = Some.Valid<NonEmptyGuid>();

		var options = new OptionsFactory<ClassArchetype.GetSetStyleClassContainingANonEmptyGuid>([], [])
			.Create(Options.DefaultName);

		var configSection = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string>
			{
				["Foo:FirstProperty"] = stringSetting,
				["Foo:SecondProperty"] = nonEmptyGuidSetting.ToString()
			})
		.Build()
		.GetSection("Foo");

		// Act
		configSection.Bind(options);

		// Assert
		var iOptions = Options.Create(options);
		using (Assert.EnterMultipleScope())
		{
			Assert.That(iOptions, Is.Not.Null);
			Assert.That(iOptions.Value, Is.Not.Null);

			var settings = iOptions.Value;
			Assert.That(settings.FirstProperty, Is.EqualTo(stringSetting));
			Assert.That(settings.SecondProperty, Is.EqualTo(nonEmptyGuidSetting));
		}
	}
}
